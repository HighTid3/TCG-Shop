using System;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.ViewModels
{
    public class DataSetViewModel
    {
        [Required]
        public string DataSet { get; set; }

        [Required]
        public string DateStart { get; set; }

        [Required]
        public string DateEnd { get; set; }
    }
}