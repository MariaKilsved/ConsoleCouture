using System;
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
                var productDetails = from product in db.Products
                                         where product.Id == id
                                         join
                                         category in db.Categories on product.CategoryId equals category.Id
                                         select new { product, category };

                var prod = productDetails.ToList();

                Console.Clear();

                Console.WriteLine($"{prod[0].product.Id,-5}{prod[0].product.Name,-85}{prod[0].product.Price:C2}");
                Console.WriteLine();

                string ascii = prod[0].category.Name switch
                {
                    "Tröjor" => Utility.ASCIIArt.Shirt(),
                    "Byxor och shorts" => Utility.ASCIIArt.Pants(),
                    "Klänningar och kjolar" => Utility.ASCIIArt.Dress(),
                    "Hattar och huvudscarves" => Utility.ASCIIArt.Hat(),
                    "Solglasögon" => Utility.ASCIIArt.Glasses(),
                    _ => Utility.ASCIIArt.Other()
                };

                Console.WriteLine(ascii);
                Console.WriteLine();
                Console.WriteLine(prod[0].product.Info);
            }

            using (var db = new Models.ConsoleCoutureContext())
            {
                var sizes = from stock in db.Stocks
                            where stock.ProductId == id && stock.InStock > 0
                            select stock;

                Console.WriteLine();
                Console.WriteLine("Tillgängliga storlekar:");
                Console.WriteLine($"{"Id",-5}{"Produkt",-85}");
                foreach (var size in sizes)
                {
                    Console.WriteLine($"{size.Id,-5}{size.Size}");
                }

            }
        }

        public static bool SelectProduct(string title, out int id)
        {
            string sInput;

            do
            {
                Console.WriteLine();
                Console.WriteLine(title);
                Console.WriteLine("Välj M för att gå tillbaka till menyn");
                Console.WriteLine("Tryck Q för att avsluta");

                sInput = Console.ReadLine();

                if (int.TryParse(sInput, out id) && id > 0 || sInput.ToUpper() == "M")
                {
                    return true;
                }
                else if (sInput != "Q" && sInput != "q")
                {
                    Console.WriteLine("Ogiltig inmatning, försök igen.");
                }

            } while (sInput != "Q" && sInput != "q");

            return false;
        }
    }
}
