using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient; //VisualStudio recommended change to this from just using Data.SqlClient

namespace ConsoleCouture
{
    class DapperAdmin
    {
        static readonly string connString = "data source=.\\SQLEXPRESS; initial catalog = ConsoleCouture; persist security info = True; Integrated Security = True;";

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

        public static bool AdminMenuOptions(out int selection)
        {
            string sInput;
            selection = 0;
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

                if (int.TryParse(sInput, out selection) && selection > 0 && selection < 4)
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

        public static bool ExecuteAdminMenu(int selection)
        {
            bool Continue = true;

            switch (selection)
            {
                case 1:
                    Continue = AddProduct();
                    break;
                case 2:
                    Continue = AddCategory();
                    break;
                case 3:
                    Continue = AddSupplier();
                    break;
                default:
                    break;
            }

            return Continue;
        }

        private static bool AddProduct()
        {
            Console.WriteLine("Lägg till produkt");
            Console.WriteLine("Skriv in Q för att avsluta");
            Console.WriteLine("Skriv in M för att gå tillbaka till huvudmenyn");

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

                if (sInput.Length > 100)
                {
                    Console.WriteLine("För långt namn, försök igen.");
                }
                else if(sInput == "M" || sInput == "m")
                {
                    return true;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    name = sInput.Trim();
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

                if (int.TryParse(sInput.Trim(), out int tempCategory) && tempCategory >= 0 && tempCategory <= categoriesList.Count)
                {
                    categoryId = tempCategory;
                    isAdding = false;
                }
                else if (sInput == "M" || sInput == "m")
                {
                    return true;
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

                if (int.TryParse(sInput.Trim(), out tempSupplier) && tempSupplier > 0 && tempSupplier <= counter)
                {
                    supplierId = tempSupplier;
                    isAdding = false;
                }
                else if (sInput == "M" || sInput == "m")
                {
                    return true;
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
            decimal tempPrice = 0m;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("sv-SE");

            Console.WriteLine();

            //Validate input
            while(price == 0 || price == null)
            {
                tempPrice = 0;
                Console.WriteLine("Mata in produktens pris:");
                sInput = Console.ReadLine();

                if (decimal.TryParse(sInput.Trim(), NumberStyles.Currency, culture, out tempPrice) && tempPrice > 0)
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
                    return true;
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

                if (sInput.Trim().Length > 1000)
                {
                    Console.WriteLine("Beskrivningen är för lång, försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    return true;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    info = sInput.Trim();
                }
            }

            //Actually add the new product
            var affectedRows = 0;

            var sql = $"INSERT INTO [Products]([Name], [CategoryId], [SupplierId], [Price], [Info]) VALUES ('{name}', {categoryId}, {supplierId}, {price}, '{info}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                    if (affectedRows > 0)
                    {
                        Console.WriteLine("Produkt tillagd.");
                        System.Threading.Thread.Sleep(3000);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    System.Threading.Thread.Sleep(7000);
                    return true;
                }
            }
            return true;
        }

        private static bool AddCategory()
        {
            Console.WriteLine("Lägg till kategori");
            Console.WriteLine("Skriv in Q för att avsluta");
            Console.WriteLine("Skriv in M för att gå tillbaka till huvudmenyn");

            string sInput; ;
            string name = "";            //varchar (30)

            //Validate input
            while (String.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Skriv in kategorins namn:");
                sInput = Console.ReadLine();

                if (sInput.Length > 30)
                {
                    Console.WriteLine("För långt namn, försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    return true;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else
                {
                    name = sInput.Trim();
                }
            }

            //Actually add the new category
            var affectedRows = 0;

            var sql = $"INSERT INTO Categories(Name) VALUES ('{name}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                    if (affectedRows > 0)
                    {
                        Console.WriteLine("Kategori tillagd.");
                        System.Threading.Thread.Sleep(3000);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    System.Threading.Thread.Sleep(7000);
                    return true;
                }
            }
            return true;
        }

        private static bool AddSupplier()
        {
            Console.WriteLine("Lägg till leverantör");
            Console.WriteLine("Skriv in Q för att avsluta");
            Console.WriteLine("Skriv in M för att gå tillbaka till huvudmenyn");
            bool returnToMenu;

            string companyName;         //varchar(50)
            string contactName;         //varchar(50)
            string contactTitle;        //varchar(50)
            string address;             //varchar(100)
            string postalCode;          //varchar(20)
            string city;                //varchar(50)
            string country;             //varchar(50)
            string phone;               //varchar(20)
            string mail;                //varchar(100)

            //Obtain input while also checking if the user wants to return to menu or quit
            companyName = ObtainStringInput("Ange företagets namn:", 50, out bool quit, out returnToMenu);
            if(returnToMenu)
            {
                return true;
            }
            if(quit)
            {
                return false;
            }
            contactName = ObtainStringInput("Ange namnet på en kontaktperson hos företaget:", 50, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            contactTitle = ObtainStringInput("Ange vilken position kontaktpersonen har hos företaget (personens jobbtitel):", 50, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            address = ObtainStringInput("Ange leveratörens gatuadress:", 100, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            postalCode = ObtainStringInput("Ange leverantörens postkod:", 20, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            city = ObtainStringInput("Ange leverantörens postort:", 50, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            country = ObtainStringInput("Ange vilket land leveratörens adress är i:", 50, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            phone = ObtainPhoneInput("Ange leverantörens telefonnummer:", 20, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }
            mail = ObtainMailInput("Ange leverantörens mailadress:", 100, out quit, out returnToMenu);
            if (returnToMenu)
            {
                return true;
            }
            if (quit)
            {
                return false;
            }

            //Actually add the new supplier
            var affectedRows = 0;

            var sql = $"INSERT INTO Suppliers(CompanyName, ContactName, ContactTitle, Address, PostalCode, City, Country, Phone, Mail) VALUES ('{companyName}', {contactName}, {contactTitle}, {address}, '{postalCode}', {city}, {country}, {phone}, '{mail}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                    if (affectedRows > 0)
                    {
                        Console.WriteLine("Leverantör tillagd.");
                        System.Threading.Thread.Sleep(3000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    System.Threading.Thread.Sleep(7000);
                }
            }
            return true;
        }


        private static string ObtainStringInput(string title, int maxLength, out bool quit, out bool returnToMenu)
        {
            string sInput = "";
            returnToMenu = false;
            quit = false;

            while (true)
            {
                Console.WriteLine(title);
                sInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
                }
                else if (sInput.Length > maxLength)
                {
                    Console.WriteLine("För lång text. Försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    quit = false;
                    break;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    returnToMenu = false;
                    quit = true;
                    break;
                }
                else
                {
                    break;
                }
            }
            return sInput;
        }


        private static string ObtainMailInput(string title, int maxLength, out bool quit, out bool returnToMenu)
        {
            returnToMenu = false;
            quit = false;
            string sInput = "";

            while (true)
            {
                Console.WriteLine(title);
                sInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    quit = false;
                    break;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    returnToMenu = false;
                    quit = true;
                    break;
                }
                else if (sInput.Length > maxLength)
                {
                    Console.WriteLine("För lång text. Försök igen.");
                }
                else if (!sInput.Contains('@') || !sInput.Contains('.'))
                {
                    Console.WriteLine("Måste vara en mailadress");
                }
                else
                {
                    break;
                }
            }
            return sInput;
        }

        private static string ObtainPhoneInput(string title, int maxLength, out bool quit, out bool returnToMenu)
        {
            returnToMenu = false;
            quit = false;
            string sInput;

            while (true)
            {
                Console.WriteLine(title);
                sInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
                }
                else if (sInput == "M" || sInput == "m")
                {
                    returnToMenu = true;
                    quit = false;
                    break;
                }
                else if (sInput == "Q" || sInput == "q")
                {
                    returnToMenu = false;
                    quit = true;
                    break;
                }
                else if (sInput.Length > maxLength)
                {
                    Console.WriteLine("För långt nummer. Försök igen.");
                }
                else if (!sInput.Contains('0') || !sInput.Contains('1') || !sInput.Contains('2') || !sInput.Contains('3') || !sInput.Contains('4') || !sInput.Contains('5') || !sInput.Contains('6') || !sInput.Contains('7') || !sInput.Contains('8') || !sInput.Contains('9'))
                {
                    Console.WriteLine("Måste vara ett nummer.");
                }
                else
                {
                    break;
                }
            }
            return sInput;
        }


    }
}
