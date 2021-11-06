using DailyWatchlistEmails.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DailyWatchlistEmails
{
    public interface IWatchlistEmailService
    {
        Task<bool> Execute();
    }

    public class WatchlistEmailService: IWatchlistEmailService
    {
        private readonly IFoolRepository _foolRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WatchlistEmailService> _log;

        public WatchlistEmailService (IFoolRepository foolRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WatchlistEmailService> log)
        {
            _foolRepository = foolRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _log = log;
        }

        private List<Article> _articles { get; set; }
        private List<ArticleInstrument> _allInstruments { get; set; }


        public async Task<bool> Execute()
        {
            return 
                await getArticles() &&
                await prepareTemplate() &&
                await sendEmails();
        }


        /// <summary>
        /// Retrieve all articles published between 5 pm yesterday and 5 pm today.
        /// </summary>
        private async Task<bool> getArticles()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime endDate = new DateTime(today.Year, today.Month, today.Day, 17, 0, 0);
                DateTime startDate = endDate.AddDays(-1);
                var articleApiUri = _configuration.GetValue<string>("ArticleApiUri");
                var uri = $"{articleApiUri}articles?publish_date_from={startDate.ToString("o")}&publish_date_to={endDate.ToString("o")}";
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    _log.LogError($"Failed to get articles.  Response code: {response.StatusCode}");
                    return false;
                }
                var json = await response.Content.ReadAsStringAsync();
                _articles = JsonConvert.DeserializeObject<List<Article>>(json);

                _log.LogInformation($"Successfully retrieved articles from {uri}");
                return true;
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "Exception executing getArticles!");
                return false;
            }
        }

        /// <summary>
        /// Replace all of the content of the template in MailChamp with information 
        /// for today's articles and instruments. MailChamp will use this in combination
        /// with each recipient's list of watched instrument ids to build their personalized
        /// email.
        /// </summary>
        private async Task<bool> prepareTemplate()
        {
            try
            {
                _allInstruments = new List<ArticleInstrument>();

                #region XML generation
                string articles = "<Articles>";
                for (int i = 0; i < _articles.Count; i++)
                {
                    Article article = _articles[i];

                    string authors = "<Authors>";
                    for (int j = 0; j < article.authors.Length; j++)
                    {
                        Author author = article.authors[j];
                        authors += string.Format("<Author>" +
                                "<FirstName>{0}</FirstName>" +
                                "<LastName>{1}</LastName>" +
                            "</Author>",
                            author.first_name,
                            author.last_name);
                    }
                    authors += "</Authors>";

                    string articleInstruments = "<InstrumentIds>";
                    for (int j = 0; j < article.instruments.Length; j++)
                    {
                        ArticleInstrument instrument = article.instruments[j];
                        articleInstruments += string.Format("<InstrumentId>{0}</InstrumentId>", instrument.instrument_id);

                        bool instrumentAlreadyAdded = false;
                        for (int k = 0; k < _allInstruments.Count; k++)
                        {
                            if (_allInstruments[k].instrument_id == instrument.instrument_id)
                            {
                                instrumentAlreadyAdded = true;
                                break;
                            }
                        }
                        if (!instrumentAlreadyAdded)
                        {
                            _allInstruments.Add(instrument);
                        }
                    }
                    articleInstruments += "</InstrumentIds>";

                    articles += string.Format("<Article>" +
                            "<Headline>{0}</Headline>" +
                            "<Byline>{1}</Byline>" +
                            "<PermaLink>{2}</PermaLink>" +
                            "<PublishDate>{3}</PublishDate>" +
                            "{4}" +
                            "{5}" +
                        "</Article>",
                        article.headline,
                        article.byline,
                        article.perma_link,
                        article.date_published.ToString("o"),
                        authors,
                        articleInstruments);
                }
                articles += "</Articles>";

                string instruments = "<Instruments>";
                for (int i = 0; i < _allInstruments.Count; i++)
                {
                    ArticleInstrument instrument = _allInstruments[i];
                    instruments += string.Format("<Instrument>" +
                            "<InstrumentId>{0}</InstrumentId>" +
                            "<Symbol>{1}</Symbol>" +
                            "<CompanyName>{2}</CompanyName>" +
                        "</Instrument>",
                        instrument.instrument_id,
                        instrument.symbol,
                        instrument.company_name);
                }
                instruments += "</Instruments>";
                #endregion XML generation

                var mailChampApiUri = _configuration.GetValue<string>("MailChampUri");
                var uri = $"{mailChampApiUri}templates/9131/content";
                var client = _httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(new
                {
                    content = articles + instruments
                });
                var response = await client.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    _log.LogError($"Failed to prepare templates.  Response code: {response.StatusCode}");
                    return false;
                }

                _log.LogInformation($"Templates prepared. Processed {_articles.Count} articles and {_allInstruments.Count} instruments.");
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception executing prepareTemplate!");
                return false;
            }

        }

        /// <summary>
        /// For each subscriber, find the intersection of their watched instruments and the instruments
        /// referenced in today's articles. That intersection determines the collection of instruments
        /// and articles to be included in their personalized email.
        /// </summary>
        private async Task<bool> sendEmails()
        {
            //Get a list of all instrument ids from the articles
            IEnumerable<int> instruments = _allInstruments.Select(i => i.instrument_id); //e.g. 1,2,3,4,5,6

            //Get a list of users and what they subscribe to
            var subscriptions = await _foolRepository.GetSubscriptions().ToListAsync();
            int batchSize = 3; //NOTE: Max value for this couldbe 1024 according to the MailChamp API documentation....
            int numberOfBatches = (int)Math.Ceiling((double)subscriptions.Count() / batchSize);
            var tasks = new List<Task<bool>>();

            for (int i = 0; i < numberOfBatches; i++)
            {
                var batch = subscriptions.Skip(i * batchSize).Take(batchSize).ToList();
                var records = new List<Dictionary<string, string>>();
                batch.ForEach(subscription =>
                {
                    var instrumentsForUser = subscription.Instruments.Split(",").Select(x => Int32.Parse(x));
                    var intersection = instruments.Intersect(instrumentsForUser);

                    Dictionary<string, string> emailSendRecord = new Dictionary<string, string>();
                    emailSendRecord.Add("email", subscription.Email);
                    emailSendRecord.Add("firstName", subscription.FirstName);
                    emailSendRecord.Add("lastName", subscription.LastName);
                    emailSendRecord.Add("instruments", String.Join(",", intersection));
                    _log.LogInformation($"Creating record for {subscription.Email} : {String.Join(",", intersection)}");

                    records.Add(emailSendRecord);
                });
                tasks.Add(sendEmail(new EmailSendRecords { records = records }));
            }

            Task<bool[]> t = Task.WhenAll(tasks);            
            try
            {
                var success = !(await t).Any(x => false); //Check if any of the sendEmail calls failed
                if(!success)
                {
                    _log.LogError("One or more of the sendEmail tasks failed!");
                    return false;
                }
            }
            catch(Exception ex)
            {
                _log.LogError(ex, "Exception executing sendEmails!");
                return false;
            }
            if(t.Status == TaskStatus.RanToCompletion)
            {
                _log.LogInformation("All sendEmails tasks were ran successfully to completion.");
                return true;                
            }
            else
            {
                _log.LogError("One or more of the sendEmail tasks did not complete. They may have timed out or were cancelled.");
                return false;                
            }
        }

        private async Task<bool> sendEmail(EmailSendRecords emailSendRecords)
        {
            try
            {
                var mailChampApiUri = _configuration.GetValue<string>("MailChampUri");
                var uri = $"{mailChampApiUri}mailings/2934/send";
                var client = _httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(emailSendRecords);
                var response = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    _log.LogError($"Failed to send email.  Response code: {response.StatusCode}");
                    return false;
                }

                _log.LogInformation($"sendEmail successfully executed for {emailSendRecords.records.Count} records.");
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception executing sendEmail!");
                throw; //This is important so as to not give the false impression this ran to completion w/o error.
            }
        }
    }

    class EmailSendRecords
    {
        public List<Dictionary<string, string>> records { get; set; }
    }
}
