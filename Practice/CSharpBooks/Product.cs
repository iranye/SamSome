﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CSharp_in_Depth_Third_Edition
namespace CSharpBooks
{
    class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        Product()
        {}

        public static List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product {Name = "West Side Story", Price = 9.99m},
                new Product {Name = "Assassins", Price = 14.99m},
                new Product {Name = "Frogs", Price = 13.99m},
                new Product {Name = "Sweeney Todd", Price = 10.99m}
            };
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Price);
        }
    }
}
