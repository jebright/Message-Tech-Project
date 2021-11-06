using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyWatchlistEmails.Models
{
    public class Article
    {
        public Guid article_id { get; set; }
        public DateTime date_published { get; set; }
        public string perma_link { get; set; }
        public string headline { get; set; }
        public string byline { get; set; }
        public ArticleInstrument[] instruments { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string slug { get; set; }
        public Author[] authors { get; set; }
    }

    public class ArticleInstrument
    {
        public int instrument_id { get; set; }
        public string symbol { get; set; }
        public string company_name { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
    }

    public class Author
    {
        public int author_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
    }
}
