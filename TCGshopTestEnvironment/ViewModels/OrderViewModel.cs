﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCGshopTestEnvironment.ViewModels
{
    public class OrderViewModel
    {
        [ScaffoldColumn(false)] public int OrderId { get; set; }

        [ScaffoldColumn(false)] public DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)] public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "House Number is required")]
        [StringLength(70)]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(70)]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DisplayName("Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)] public decimal Total { get; set; }

        public List<ProductsShopCartViewModel> OrderDetails { get; set; }

        //public string Name { set; get; }
        //public string Value { set; get; }
    }
}