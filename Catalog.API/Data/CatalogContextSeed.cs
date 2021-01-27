using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(x => true).Any();
            if (!existProduct)
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Iphone 12",
                    Summary="Ios",
                    Description="test",
                    ImageFile="product-1.png",
                    Price=100,
                    Category="Smart Phone"
                },
                new Product()
                {
                    Name = "Iphone x",
                    Summary="Ios",
                    Description="test",
                    ImageFile="product-2.png",
                    Price=123,
                    Category="Smart Phone"
                },
                new Product()
                {
                    Name = "Iphone xs",
                    Summary="Ios",
                    Description="test",
                    ImageFile="product-3.png",
                    Price=666,
                    Category="Smart Phone"
                },
                new Product()
                {
                    Name = "Galaxy 12",
                    Summary="Android",
                    Description="test",
                    ImageFile="product-4.png",
                    Price=750,
                    Category="Smart Phone"
                }
            };
        }
    }
}