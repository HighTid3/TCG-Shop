using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.ViewModels
{
    public class ChartViewModel
    {
        [Required]
        public List<DataSetViewModel> DataSetViewModels { set; get; }

    }
}