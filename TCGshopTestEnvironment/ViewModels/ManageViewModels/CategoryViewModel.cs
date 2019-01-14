using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCGshopTestEnvironment.ViewModels.ManageViewModels
{
    public class CategoryViewModel
    {
        [Required]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}
