using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;
using TCGshopTestEnvironment.ViewModels;

namespace TCGshopTestEnvironment.Services
{
    //all possible methods for gaining/writing data to the products table in the database
    public interface IProducts
    {
        IEnumerable<Products> GetAll();
        Products GetByID(int id);
        void Add(Products NewProduct);
        string GetName(int id);

        IQueryable<Productsandcategorie> GetbyCardType(string type);

        IQueryable<Productsandcategorie> GetByNameSearch(string name);

        List<string> GetCardCatagory(IEnumerable<Products> cards);

    }
}
