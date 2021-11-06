using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DailyWatchlistEmails
{
    [Table("UserEmailSubscription", Schema = "dbo")]
    public class UserEmailSubscription
    {
        [Key]
        public int UserId { get; set; }
        public int EmailSubscriptionId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
