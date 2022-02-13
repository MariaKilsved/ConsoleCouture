using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConsoleCouture
{
    class Admin
    {

        //- Kunna skapa produktkategorier
        //- Kunna lägga till produkter och kategorier
        //- Kunna se försäljningsinformation
        //- Kunna ta ut statistik om t.ex. mest säljande produktkategorier

        //- Bäst säljande produkter
        //- Populäraste produkt för man/kvinna
        //- Populäraste produkt i olika åldersgrupper
        //- Flest beställningar per stad
        //- Försäljning sorterat på leverantörer
        //- Populäraste kategorin

        public static bool AdminMenu()
        {
            string sInput;
            Console.Clear();

            do
            {
                Console.WriteLine();
                Console.WriteLine("Alternativ:");
                Console.WriteLine("[1] Lägg till produkt");
                Console.WriteLine("[2] Lägg till produktkategori");
                Console.WriteLine("[3] Lägg till leverantör");
                Console.WriteLine("[Q] Avsluta");

                sInput = Console.ReadLine();

                if (int.TryParse(sInput, out int selection) && selection > 0 && selection < 4)
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

        private static bool AddProduct(out bool returnToMenu)
        {
            Console.WriteLine("---Lägg till produkt---");
            Console.WriteLine("Skriv in Q för att avsluta");
            Console.WriteLine("Skriv in M för att gå tillbaka till huvudmenyn");

            returnToMenu = false;
            bool isAdding = true;
            string sInput = "";
            //int id = 0;
            string name = "";
            int? categoryId = null;
            int? supplierId = null;
            decimal? price = null;
            string info = null;

            //Name: varchar(100)
            //Price: smallmoney
            //Info: varchar(1000)

            // Name
            while (String.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Skriv in produktens namn:");
                sInput = Console.ReadLine();
                sInput.Trim();

                if (sInput.Length > 100)
                {
                    Console.WriteLine("För långt namn, försök igen.");
                }
                else if(sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    return false;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    name = sInput;
                }
            }
            //---categoryId---
            string categoriesString = "Tillgängliga kategorier:\n";
            categoriesString += $"{"0",-5}{"Utan kategori"}\n";
            sInput = "";
            List<Models.Category> categoriesList;

            //Get categories
            using (var db = new Models.ConsoleCoutureContext())
            {
                var categories = from category in db.Categories
                                    select category;

                categoriesList = categories.ToList();

                foreach(var category in categoriesList)
                {
                    categoriesString += $"{category.Id, -5}{category.Name}\n";
                }
            }

            //Validate input
            Console.WriteLine();
            Console.WriteLine(categoriesString);
            while (isAdding)
            {
                Console.WriteLine("Mata in en siffra som motsvarar en kategori.");
                sInput = Console.ReadLine();
                sInput.Trim();

                if (int.TryParse(sInput, out int tempCategory) && tempCategory >= 0 && tempCategory <= categoriesList.Count())
                {
                    categoryId = tempCategory;
                    isAdding = false;
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    return false;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning, försök igen.");
                }
            }

            //---supplierId---
            string supplierString = "Tillgängliga leverantörer:\n";
            sInput = "";
            isAdding = true;
            int tempSupplier;
            int counter = 0;


            //Get suppliers
            using (var db = new Models.ConsoleCoutureContext())
            {
                var suppliers = from supplier in db.Suppliers
                                    select new {supplier.Id, supplier.CompanyName};

                foreach (var supplier in suppliers)
                {
                    supplierString += $"{supplier.Id,-5}{supplier.CompanyName}\n";
                    counter++;
                }
            }

            //Validate input
            Console.WriteLine();
            Console.WriteLine(supplierString);
            while (isAdding)
            {
                Console.WriteLine("Mata in en siffra som motsvarar en leverantör.");
                sInput = Console.ReadLine();
                sInput.Trim();

                if (int.TryParse(sInput, out tempSupplier) && tempSupplier > 0 && tempSupplier <= counter)
                {
                    supplierId = tempSupplier;
                    isAdding = false;
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    return false;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning, försök igen.");
                }
            }

            //---Price---
            sInput = "";
            decimal tempPrice = 0;

            Console.WriteLine();

            //Validate input
            while(price == 0 || price == null)
            {
                tempPrice = 0;
                Console.WriteLine("Mata in produktens pris:");
                sInput = Console.ReadLine();
                sInput.Replace(',', '.').Replace(';', '.').Replace(':', '.').Replace("kr", "").Replace("Kr", "").Replace(":-", "");
                sInput.Trim();

                if (decimal.TryParse(sInput, out tempPrice) && tempPrice > 0)
                {
                    if(tempPrice > 214748)
                    {
                        Console.WriteLine("Pris för högt, försök igen.");
                    }
                    else
                    {
                        price = tempPrice;
                    }
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    return false;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning, försök igen.");
                }
            }

            //Info
            sInput = "";
            while (String.IsNullOrWhiteSpace(info))
            {
                Console.WriteLine("Skriv in en beskrivning av produkten:");
                sInput = Console.ReadLine();
                sInput.Trim();

                if (sInput.Length > 1000)
                {
                    Console.WriteLine("Beskrivningen är för lång, försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    return false;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    info = sInput;
                }
            }

            //Actually add the new product

            //var prod = new { Name = name, CategoryId = categoryId, SupplierId = supplierId, Price = price, Info = info };


            var sql = $"INSERT INTO Products(Name, CategoryId, SupplierId, Price, Info) VALUES ('{name}', '{categoryId}', '{supplierId}', '{price}', '{info})";

            /*
            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    var affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            */






            return false;
        }
    }
}
