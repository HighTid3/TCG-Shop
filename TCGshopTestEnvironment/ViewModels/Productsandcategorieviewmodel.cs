using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.Models.JoinTables;

namespace TCGshopTestEnvironment.ViewModels
{
    public class Productsandcategorie
    {
        public List<string> Catnames { get; set; }

        public ProductsCat prods { get; set; }
    }
}
