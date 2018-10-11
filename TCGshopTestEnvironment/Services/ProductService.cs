using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TCGshopTestEnvironment.Models;

namespace TCGshopTestEnvironment.Services
{
    public class ProductService : IProducts
    {
        private DBModel _context;

        public ProductService(DBModel context)
        {
            _context = context;
        }

        public void Add(Products NewProduct)
        {
            _context.Add(NewProduct);
            _context.SaveChanges();
            
        }

        public IEnumerable<Products> GetAll()
        {
            return _context.products;
        }

        public Products GetByID(int id)
        {
            
            return _context.products
                    .FirstOrDefault(product => product.ProductId == id);
        }

        public string GetName(int id)
        {
            return _context.products.FirstOrDefault(product => product.ProductId == id).Name;
        }

        public IEnumerable<Products> GetbyCardType(string type)
        {

            return from p in _context.products
                join c in _context.ProductCategory on p.ProductId equals c.ProductId where c.CategoryName == type
                select p;
        }

        public IEnumerable<Products> GetByNameSearch(string name)
        {
            return from p in _context.products
                where p.Name.ToLower() == name || p.Name.ToLower().Contains(name)
                select p;
        }
    }
}
