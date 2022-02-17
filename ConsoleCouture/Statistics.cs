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
                Console.WriteLine("[M] Tillbaka till huvudmenyn");
                Console.WriteLine("[Q] Avsluta");

                sInput = Console.ReadLine();


                if (int.TryParse(sInput, out selection) && selection > 0 && selection < 7)
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
                                          group prods by prods.ProductName).ToList();

                Console.WriteLine();
                Console.WriteLine("Sålda produkter");
                Console.WriteLine($"{"Produktnamn",-60}{"Antal",-20}{"Totalt pris"}");
                foreach (var group in groupedProdDetails)
                {
                    long sumPrice = group.Sum(prod => (long)prod.Price);
                    long sumQuantity = group.Sum(prod => (long)prod.Quantity);

                    Console.WriteLine($"{group.Key,-60}{(sumQuantity + " st"),-20}{sumPrice:C2}");
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
                                  select new { ProductName = product.Name, Price = product.Price, Quantity = stock.InStock, TotalPrice = (long)(product.Price * stock.InStock) };


                var groupedProdStock = (from prods in prodStock.ToList()
                                          group prods by prods.ProductName).ToList();


                long balance = 0;
                Console.WriteLine();
                Console.WriteLine("Lagersaldo (värde av produkter på lager):");
                Console.WriteLine($"{"Produktnamn",-60}{"Totalt värde"}");
                foreach (var group in groupedProdStock)
                {
                    long sumPrice = group.Sum(prod => prod.TotalPrice);
                    balance += sumPrice;
                    Console.WriteLine($"{group.Key,-60}{sumPrice:C2}");
                }

                Console.WriteLine();
                Console.WriteLine($"Totalt lagersaldo (värdet av alla produkter på lager): {balance:C2}");
            }
        }


    }
}
