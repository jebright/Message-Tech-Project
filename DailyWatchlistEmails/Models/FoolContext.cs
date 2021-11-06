using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyWatchlistEmails.Models
{

    public interface IFoolContext
    {
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Instrument> Instruments { get; set; }
        DbSet<UserWatchedInstrument> UserWatchedInstruments { get; set; }
        DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        DbSet<UserEmailSubscription> UserEmailSubscriptions { get; set; }
    }

    public class FoolContext : DbContext, IFoolContext
    {
        private readonly IConfiguration _configuration;
        public FoolContext(IConfiguration configuration): base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Fool"));
        }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<UserWatchedInstrument> UserWatchedInstruments { get; set; }
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<UserEmailSubscription> UserEmailSubscriptions { get; set; }
    }
}
