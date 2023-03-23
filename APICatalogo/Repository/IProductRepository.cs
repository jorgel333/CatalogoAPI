﻿using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetOrderProductsByPrice();
    }
}
