using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture
{
    class Statistics
    {

        public static bool StatisticsMenu()
        {

            string sInput;
            int selection = 0;
            Console.Clear();

            while(true)
            {
                Console.WriteLine("Alternativ:");
                Console.WriteLine("[1] Försäljningsinformation");
                Console.WriteLine("[2] Lagersaldo");
                Console.WriteLine("[3] Produkters popularitet sorterat på stad");
                Console.WriteLine("[4] Produkters popularitet sorterat på kön");
                Console.WriteLine("[5] Produkters popularitet sorterat på åldersgrupp");
                Console.WriteLine("[6] Försäljningsinformation sorterat på kategori");
                Console.WriteLine("[7] Försäljningsinformation sorterat på leverantör");
                Console.WriteLine("[8] Avslöja män som köpte klänningar eller kjolar");         //Hahahaha
                Console.WriteLine("[9] Kontaktuppgifter till lönsamma kunder");
                Console.WriteLine("[10] Kontaktuppgifter till de senast aktiva kunderna");
                Console.WriteLine("[M] Tillbaka till huvudmenyn");
                Console.WriteLine("[Q] Avsluta");

                sInput = Console.ReadLine();


                if (int.TryParse(sInput, out selection) && selection > 0 && selection < 11)
                {
                    ExecuteStatisticsMenu(selection);
                    Console.WriteLine();
                }
                else if(sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else if(sInput ==  "M" || sInput == "m")
                {
                    return true;
                }
                else 
                {
                    Console.WriteLine("Ogiltig inmatning, försök igen.");
                    Console.WriteLine();
                }

            } 

        }

        public static bool ExecuteStatisticsMenu(int selection)
        {
            bool Continue = true;

            switch (selection)
            {
                case 1:
                    PrintSales();
                    Console.WriteLine();
                    break;
                case 2:
                    PrintBalance();
                    Console.WriteLine();
                    break;
                case 3:
                    PrintPopularityByCity();
                    Console.WriteLine();
                    break;
                case 4:
                    PrintPopularityByGender();
                    Console.WriteLine();
                    break;
                case 5:
                    PrintPopularityByAge();
                    Console.WriteLine();
                    break;
                case 6:
                    PrintSalesByCategory();
                    Console.WriteLine();
                    break;
                case 7:
                    PrintSalesBySupplier();
                    Console.WriteLine();
                    break;
                case 8:
                    PrintMenBuyingWomensClothing();
                    Console.WriteLine();
                    break;
                case 9:
                    PrintRichCustomers();
                    Console.WriteLine();
                    break;
                case 10:
                    PrintActiveCustomers();
                    Console.WriteLine();
                    break;
                default:
                    break;
            }

            return Continue;
        }

        private static void PrintSales()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var prodDetails = from
                                    product in db.Products
                                  join
                                    details in db.OrderDetails on product.Id equals details.ProductId
                                  select new Models.OrderDetailsProductQuery { ProductName = product.Name, Price = product.Price, Quantity = details.Quantity };


                var groupedProdDetails = (from prods in prodDetails.ToList()
                                          where prods.Quantity > 0
                                          orderby (prods.Price * prods.Quantity) descending
                                          group prods by prods.ProductName).ToList();

                Console.WriteLine();
                Console.WriteLine("Sålda produkter");
                Console.WriteLine($"{"Produktnamn",-85}{"Antal",-20}{"Totalt pris"}");
                foreach (var group in groupedProdDetails)
                {
                    long sumPrice = group.Sum(prod => (long)prod.Price);
                    long sumQuantity = group.Sum(prod => (long)prod.Quantity);

                    Console.WriteLine($"{group.Key,-85}{(sumQuantity + " st"),-20}{sumPrice:C2}");
                }
            }
        }

        private static void PrintBalance()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var prodStock = from
                                    product in db.Products
                                  join
                                    stock in db.Stocks on product.Id equals stock.ProductId
                                  where stock.InStock > 0 && product.Price > 0
                                  select new { ProductName = product.Name, Quantity = stock.InStock, TotalPrice = (long)(product.Price * stock.InStock) };


                var groupedProdStock = (from prods in prodStock.ToList()
                                          orderby prods.TotalPrice descending
                                          group prods by prods.ProductName).ToList();


                long balance = 0;
                Console.WriteLine();
                Console.WriteLine("Lagersaldo (värde av produkter på lager):");
                Console.WriteLine($"{"Produktnamn",-85}{"Totalt värde"}");
                foreach (var group in groupedProdStock)
                {
                    long sumPrice = group.Sum(prod => prod.TotalPrice);
                    balance += sumPrice;
                    Console.WriteLine($"{group.Key,-85}{sumPrice:C2}");
                }

                Console.WriteLine();
                Console.WriteLine($"Totalt lagersaldo (värdet av alla produkter på lager): {balance:C2}");
            }
        }

        private static void PrintPopularityByCity()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                                    product in db.Products
                                  join
                                    details in db.OrderDetails on product.Id equals details.ProductId
                                  join
                                    orders in db.Orders on details.OrderId equals orders.Id
                                  where details.Quantity > 0
                                  select new { ProductName = product.Name, Quantity = details.Quantity, City = orders.ShipCity, TotalPrice = (long)(product.Price * details.Quantity) };


                var groupedResult = (from prods in result.ToList()
                                          orderby prods.City
                                          group prods by prods.City).ToList();

                Console.WriteLine();
                Console.WriteLine("Mest sålda produkter per stad:");
                foreach (var group in groupedResult)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}:");
                    Console.WriteLine($"{"Produktnamn",-85}{"Antal",-20}{"Totalt pris"}");
                    foreach (var pd in group)
                    {
                        Console.WriteLine($"{pd.ProductName,-85}{(pd.Quantity + " st"), -20}{pd.TotalPrice:C2}");
                    }
                }
            }
        }

        private static void PrintPopularityByGender()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                               product in db.Products
                             join
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join
                               orders in db.Orders on details.OrderId equals orders.Id
                             join
                               customers in db.Customers on orders.CustomerId equals customers.Id
                             where details.Quantity > 0 && customers.Gender != null
                             select new { ProductName = product.Name, Quantity = details.Quantity, TotalPrice = (long)(product.Price * details.Quantity), Gender = customers.Gender };

                var groupedResult = (from prods in result.ToList()
                                     orderby prods.Gender, prods.ProductName
                                     group prods by prods.Gender).ToList();


                Console.WriteLine();
                Console.WriteLine("Mest sålda produkter per kön:");
                foreach (var group in groupedResult)
                {
                    Console.WriteLine();
                    if(group.Key.Contains('M'))
                    {
                        Console.WriteLine("---Män---");
                    }
                    else if (group.Key.Contains('F'))
                    {
                        Console.WriteLine("---Kvinnor---");
                    }
                    else if (group.Key.Contains("NB"))
                    {
                        Console.WriteLine("---Icke-binära---");
                    }
                    else
                    {
                        Console.WriteLine("---Odefinierade---");
                    }

                    Console.WriteLine($"{"Produktnamn",-85}{"Antal",-20}{"Totalt pris"}");
                    foreach (var pd in group)
                    {
                        Console.WriteLine($"{pd.ProductName,-85}{(pd.Quantity + " st"),-20}{pd.TotalPrice:C2}");
                    }
                }
            }
        }

        private static string CalculateAgeGroup(DateTime? birthdate)
        {
            //Calculate exact age:
            DateTime birthdate2 = birthdate ?? DateTime.MinValue;


            var today = DateTime.Today;
            var age = today.Year - birthdate2.Year;

            if (birthdate2.Date > today.AddYears(-age))
            {
                age--;
            }

            string sRet = "";
            int agegroup = (int)Math.Floor((decimal)age / 10) * 10;

            if (agegroup == 0 || agegroup == 10)
            {
                sRet = "Barn och unga";
            }
            else if(agegroup > 200)
            {
                sRet += "Uppgifter saknas";
            }
            else
            {
                sRet += $"{agegroup}-{agegroup + 10} år";
            }


            return sRet;
        }

        private static void PrintPopularityByAge()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                                   product in db.Products
                             join
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join 
                               orders in db.Orders on details.OrderId equals orders.Id
                             join
                               customers in db.Customers on orders.CustomerId equals customers.Id
                             where details.Quantity > 0
                             select new { BirthDate = CalculateAgeGroup(customers.BirthDate), ProductName = product.Name, Quantity = details.Quantity, TotalPrice = (long)(product.Price * details.Quantity) };


                var groupedResult = (from prods in result.ToList()
                                     orderby prods.BirthDate, prods.TotalPrice descending
                                     group prods by prods.BirthDate).ToList();

                Console.WriteLine();
                Console.WriteLine("Mest sålda produkter per åldersgrupp:");
                foreach (var group in groupedResult)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}:");
                    Console.WriteLine($"{"Produktnamn",-85}{"Antal",-20}{"Totalt pris"}");
                    foreach (var pd in group)
                    {
                        Console.WriteLine($"{pd.ProductName,-85}{(pd.Quantity + " st"),-20}{pd.TotalPrice:C2}");
                    }
                }
            }
        }
        private static void PrintSalesByCategory()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                               product in db.Products
                             join
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join
                               orders in db.Orders on details.OrderId equals orders.Id
                             join
                               categories in db.Categories on product.CategoryId equals categories.Id
                             where details.Quantity > 0 && categories.Name != null
                             select new { ProductName = product.Name, Quantity = details.Quantity, Category = categories.Name, TotalPrice = (long)(product.Price * details.Quantity) };


                var groupedResult = (from prods in result.ToList()
                                     orderby prods.Category
                                     group prods by prods.Category).ToList();

                Console.WriteLine();
                Console.WriteLine("Kategoriernas popularitet:");
                foreach (var group in groupedResult)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}:");

                    long sumQuantity = group.Sum(prod => (long)prod.Quantity);
                    long sumPrice = group.Sum(prod => prod.TotalPrice);

                    Console.WriteLine($"{sumQuantity} enheter sålda.");
                    Console.WriteLine($"{sumPrice} totalt pris.");
                }
            }
        }
        private static void PrintSalesBySupplier()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                                   product in db.Products
                             join
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join
                               supplier in db.Suppliers on product.SupplierId equals supplier.Id
                             where details.Quantity > 0 && supplier.CompanyName != null
                             select new { SupplierCompany = supplier.CompanyName, TotalPrice = (long)(product.Price * details.Quantity) };

                var groupedResult = (from prods in result.ToList()
                                     group prods by prods.SupplierCompany).ToList();

                Console.WriteLine();
                Console.WriteLine("Värdet av alla leverantörers produkter:");


                foreach (var group in groupedResult)
                {
                    long sumPrice = group.Sum(prod => prod.TotalPrice);

                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}: {sumPrice:C2}");
                }
            }
        }
        private static void PrintMenBuyingWomensClothing()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                //Because of INNER JOIN, only the customers who bought Skirts/dresses will be included...
                var result = from
                               product in db.Products
                             join
                               categories in db.Categories on product.CategoryId equals categories.Id
                             join
                               orders in db.Orders on product.Id equals orders.Id
                             join 
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join
                               customers in db.Customers on orders.CustomerId equals customers.Id
                             where categories.Name == "Klänningar och kjolar"
                             select new { CustomerName = (customers.FirstName + " " + customers.LastName), ProductName = product.Name };

                var groupedResult = (from clients in result.ToList()
                                     group clients by clients.CustomerName).ToList();

                Console.WriteLine();
                Console.WriteLine("Män som köpte klänningar eller kjolar:");

                if(groupedResult.Count == 0)
                {
                    Console.WriteLine("Ingen data ännu.");
                }


                foreach (var group in groupedResult)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{group.Key} köpte:");

                    foreach(var x in group)
                    {
                        Console.WriteLine(x.ProductName);
                    }
                }
            }
        }

        private static void PrintRichCustomers()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                               product in db.Products
                             join
                               details in db.OrderDetails on product.Id equals details.ProductId
                             join
                               orders in db.Orders on details.OrderId equals orders.Id
                             join
                               customers in db.Customers on orders.CustomerId equals customers.Id
                             where details.Quantity > 0
                             select new { Name = customers.FirstName + " " + customers.LastName, Mail = customers.Mail, Phone = customers.Phone, TotalPrice = (long)(details.UnitPrice * details.Quantity)};

                var groupedResult = (from prods in result.ToList()
                                     orderby prods.TotalPrice descending
                                     group prods by prods.Name).Take(100).ToList();

                Console.WriteLine();
                Console.WriteLine("Top 100 storhandlande kunder:");


                foreach (var group in groupedResult)
                {
                    long sumTotalPrice = group.Sum(prod => prod.TotalPrice);

                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}:");
                    Console.WriteLine($"Total spendering: {sumTotalPrice}");

                    var iGroup = group.ToList();
                    Console.WriteLine(iGroup[^1].Phone);
                    Console.WriteLine(iGroup[^1].Mail);
                }
            }
        }

        private static void PrintActiveCustomers()
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var result = from
                               product in db.Products
                              join
                                details in db.OrderDetails on product.Id equals details.ProductId
                              join
                                orders in db.Orders on details.OrderId equals orders.Id
                              join
                                customers in db.Customers on orders.CustomerId equals customers.Id
                              where details.Quantity > 0
                              select new { Name = customers.FirstName + " " + customers.LastName, Mail = customers.Mail, Phone = customers.Phone, OrderDate = orders.OrderDate };

                var groupedResult = (from prods in result.ToList()
                                     orderby prods.OrderDate descending
                                     group prods by prods.Name).Take(100).ToList();

                Console.WriteLine();
                Console.WriteLine("Top 100 nyligen aktiva kunder:");


                foreach (var group in groupedResult)
                {

                    Console.WriteLine();
                    Console.WriteLine($"{group.Key}:");
                    var iGroup = group.ToList();
                    Console.WriteLine("Tel: " + iGroup[^1].Phone);
                    Console.WriteLine("Email: " + iGroup[^1].Mail);

                    DateTime date = iGroup[^1].OrderDate ?? DateTime.MinValue;
                    if(date != DateTime.MinValue)
                    {
                        Console.WriteLine($"Senast aktiv: {date.ToString("dd-MM-yyyy")}");
                    }
                }
            }
        }
    }
}
