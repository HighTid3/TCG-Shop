using System;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.Models
{
    public class Statistics
    {
        [Key]
        public int Static_ID { get; set; }

        public DateTime Date { get; set; }
    }
}