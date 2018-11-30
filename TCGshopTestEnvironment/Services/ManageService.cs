﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels.ManageViewModels;

namespace TCGshopTestEnvironment.Services
{
    public class ManageService : IManage
    {
        private static DBModel _context;
        public ManageService(DBModel context)
        {
            _context = context;
        }

        public IEnumerable<OrderOverviewViewModel> OrderOverview(string useremail)
        {
            return from p in _context.Orders where p.Email == useremail
                select new OrderOverviewViewModel
                {
                    OrderDate = p.OrderDate,
                    Ordernr = p.Guid.ToString(),
                    Status = p.PaymentStatus,
                    TotalPrice = p.Total,
                    OrderId = p.OrderId
                };
        }

        public OrderDetailsViewModel Orderdetails(string useremail, int OrderId)
        {
            return (from p in _context.Orders
                let orderdetails = (from d in _context.OrderDetails
                    join c in _context.products on d.ProductId equals c.ProductId
                    
                    where d.OrderId == OrderId
                    select new OrderViewModel
                    {
                        ImageUrl = c.ImageUrl,
                        ProductName = c.Name,
                        Grade = c.Grade,
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        TotalPrice = d.UnitPrice * d.Quantity
                    }).ToList()
                where p.Email == useremail
                select new OrderDetailsViewModel
                {
                    City = p.City,
                    Country = p.Country,
                    Email = p.Email,
                    FirstName = p.FirstName,
                    Address = p.Address,
                    LastName = p.LastName,
                    OrderDate = p.OrderDate,
                    Orderdetails = orderdetails,
                    Ordernr = p.Guid.ToString(),
                    Orderstatus = p.PaymentStatus,
                    PostalCode = p.PostalCode,
                    State = p.State,
                    Total = p.Total

                }).FirstOrDefault();
        }

    }

}
