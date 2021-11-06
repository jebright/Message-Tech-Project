using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyWatchlistEmails.Models
{
    [Table("UserWatchedInstrument", Schema = "dbo")]
    public class UserWatchedInstrument
    {
        [Key]
        public int UserId { get; set; }
        public int InstrumentId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}