using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyWatchlistEmails.Models
{

    public interface IFoolRepository
    {
        IQueryable<Subscription> GetSubscriptions();
    }

    public class FoolRepository: IFoolRepository
    {
        private readonly IFoolContext _foolContext;
        public FoolRepository(IFoolContext foolContext)
        {
            _foolContext = foolContext;
        }

        public IQueryable<Subscription> GetSubscriptions()
        {
            return _foolContext.Subscriptions.OrderBy(o => o.UserId);
        }

    }
}
