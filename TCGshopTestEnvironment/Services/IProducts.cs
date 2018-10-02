using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCGshopTestEnvironment.Models;

namespace TCGshopTestEnvironment.Services
{
    public interface IProducts
    {
        IEnumerable<Products> GetAll();
        Products GetByID(int id);
        void Add(Products NewProduct);
        string GetName(int id);



    }
}
