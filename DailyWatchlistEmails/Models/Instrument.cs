using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DailyWatchlistEmails
{
    [Table("Instrument", Schema = "dbo")]
    public class Instrument
    {
        [Key]
        public int InstrumentId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
    }
}
