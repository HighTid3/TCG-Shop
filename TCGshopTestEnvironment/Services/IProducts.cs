using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;

namespace TCGshopTestEnvironment.Services
{
    //all possible methods for gaining/writing data to the products table in the database
    public interface IProducts
    {
        IEnumerable<Products> GetAll();
        Products GetByID(int id);
        void Add(Products NewProduct);
        string GetName(int id);

        IEnumerable<Products> GetbyCardType(string type);

    }
}
