﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture
{
    class ProductView
    {
        public static void ListAllProducts()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                                     select new { product.Id, product.Name, product.Price };

                Console.Clear();
                Console.WriteLine($"{"Id",-5}{"Produkt",-85}{"Pris"}");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id,-5}{product.Name,-85}{product.Price:C2}");
                }
            }
        }

        public static void ListProductsByCategory()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                               join
                               category in db.Categories on product.CategoryId equals category.Id
                               where product.Price != null && product.CategoryId != null
                               select new Models.ProductsByCategoryQuery{ ProductId = product.Id, ProductName = product.Name, CategoryId = category.Id, CategoryName = category.Name, Price = product.Price };

                var groupedProducts = products.AsEnumerable().GroupBy(p => p.CategoryId).ToList();

                Console.Clear();
                foreach (var cGroup in groupedProducts)
                {
                    Console.WriteLine();
                    Console.WriteLine(cGroup.ToList()[0].CategoryName); 
                    Console.WriteLine($"{"Id",-5}{"Produkt",-85}{"Pris"}");
                    foreach(var product in cGroup)
                    {
                        Console.WriteLine($"{product.ProductId,-5}{product.ProductName,-85}{product.Price:C2}");
                    }
                }
            }
        }

        public static void ListProductsByPriceLowToHigh()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                                     where product.Price != null
                                     orderby product.Price
                                     select new { product.Id, product.Name, product.Price };

                Console.Clear();
                Console.WriteLine($"{"Id",-5}{"Produkt",-85}{"Pris"}");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id,-5}{product.Name,-85}{product.Price:C2}");
                }
            }
        }

        public static void ListProductsByPriceHighToLow()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                                     where product.Price != null
                                     orderby product.Price descending
                                     select new { product.Id, product.Name, product.Price };

                Console.Clear();
                Console.WriteLine($"{"Id",-5}{"Produkt",-85}{"Pris"}");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id,-5}{product.Name,-85}{product.Price:C2}");
                }
            }
        }

        public static void SearchProductsByName(string sString)
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                               where product.Name.Contains(sString)
                               select new { product.Id, product.Name, product.Price };

                Console.Clear();
                Console.WriteLine($"{"Id",-5}{"Produkt",-85}{"Pris"}");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id,-5}{product.Name,-85}{product.Price:C2}");
                }
            }
        }

        public static void GetSingleProduct(int id)
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var productAndCategory = from product in db.Products
                               join
                               category in db.Categories on product.CategoryId equals category.Id
                               select new { product, category };

                Console.Clear();

                foreach(var prodCat in productAndCategory)
                {
                    Console.WriteLine($"{prodCat.product.Id,-5}{prodCat.product.Name,-85}{prodCat.product.Price:C2}");

                    var asciiArt = new Utility.ASCIIArt();

                    string ascii = prodCat.category.Name switch
                    {
                        "Tröjor" => asciiArt.Shirt(),
                        "Byxor och shorts" => asciiArt.Pants(),
                        "Klänningar och kjolar" => asciiArt.Dress(),
                        "Hattar och huvudscarves" => asciiArt.Hat(),
                        "Solglasögon" => asciiArt.Glasses(),
                        _ => asciiArt.Other()
                    };
                    Console.WriteLine(ascii);
                    Console.WriteLine(prodCat.product.Info);
                }

            }
        }
    }
}
