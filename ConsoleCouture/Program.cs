using System;

namespace ConsoleCouture
{
    class Program
    {
        static void Main(string[] args)
        {
            int sizeId = 0;
            bool Continue;
            string title = "Skriv in Id:et för en produkt för mer information";
            string title2 = "Skriv in id:et för en produkt för att lägga till den till kundvagnen";
            Cart cart = new();

            do
            {
                Continue = MainMenu(out int menuSelection);
                sizeId = 0;
                if (!Continue) return;

                switch (menuSelection)
                {
                    case 1:
                        ProductView.ListAllProducts();
                        Continue = ProductView.SelectProduct(title, out int productId);
                        if(productId > 0)
                        {
                            ProductView.GetSingleProduct(productId);
                            Continue = ProductView.SelectProduct(title2, out sizeId);
                        }
                        break;
                    case 2:
                        ProductView.ListProductsByCategory();
                        Continue = ProductView.SelectProduct(title, out productId);
                        if (productId > 0)
                        {
                            ProductView.GetSingleProduct(productId);
                            Continue = ProductView.SelectProduct(title2, out sizeId);
                        }
                        break;
                    case 3:
                        ProductView.ListProductsByPriceLowToHigh();
                        Continue = ProductView.SelectProduct(title, out productId);
                        if (productId > 0)
                        {
                            ProductView.GetSingleProduct(productId);
                            Continue = ProductView.SelectProduct(title2, out sizeId);
                        }
                        break;
                    case 4:
                        ProductView.ListProductsByPriceHighToLow();
                        Continue = ProductView.SelectProduct(title, out productId);
                        if (productId > 0)
                        {
                            ProductView.GetSingleProduct(productId);
                            Continue = ProductView.SelectProduct(title2, out sizeId);
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Sök produkt efter namn:");
                        ProductView.SearchProductsByName(Console.ReadLine());
                        Continue = ProductView.SelectProduct(title, out productId);
                        if (productId > 0)
                        {
                            ProductView.GetSingleProduct(productId);
                            Continue = ProductView.SelectProduct(title2, out sizeId);
                        }
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine(cart.ToString());
                        Console.ReadKey();
                        break;
                    case 7:
                        Console.Clear();
                        DapperUser.RegisterUser();
                        break;
                    case 8:
                        Console.Clear();
                        Continue = DapperAdmin.AdminMenuOptions(out int selection);
                        if(!Continue)
                        {
                            break;
                        }
                        Continue = DapperAdmin.ExecuteAdminMenu(selection);
                        break;
                    default:
                        break;
                }

                if(sizeId > 0)
                {
                    cart.AddNewProduct(sizeId);
                }

            } while (Continue);

            if (!Continue) return;
        }

        private static bool MainMenu(out int selection)
        {
            string sInput;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═╗┌─┐┌┐┌┌─┐┌─┐┬  ┌─┐  ╔═╗┌─┐┬ ┬┌┬┐┬ ┬┬─┐┌─┐");
            Console.WriteLine("║  │ ││││└─┐│ ││  ├┤   ║  │ ││ │ │ │ │├┬┘├┤");
            Console.WriteLine("╚═╝└─┘┘└┘└─┘└─┘┴─┘└─┘  ╚═╝└─┘└─┘ ┴ └─┘┴└─└─┘");
            Console.ResetColor();

            do
            {
                Console.WriteLine();
                Console.WriteLine("[1] Visa alla produkter");
                Console.WriteLine("[2] Visa produkter sorterade efter kategori");
                Console.WriteLine("[3] Visa produkter sorterade efter pris lågt till högt");
                Console.WriteLine("[4] Visa produkter sorterade efter pris högt till lågt");
                Console.WriteLine("[5] Sök produkt");
                Console.WriteLine("[6] Visa kundvagnen");
                Console.WriteLine("[7] Registrera ny användare");
                Console.WriteLine("[8] Administration");
                Console.WriteLine("[Q] Avsluta");

                //Att göra: rea, populärast, nyast
                //Bör ändra query:s så att bara top 100 eller något visas åt gången?

                sInput = Console.ReadLine();

                if(int.TryParse(sInput, out selection) && selection > 0 && selection < 9)
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


//Server=tcp:mariak.database.windows.net,1433;Initial Catalog=ConsoleCouture;Persist Security Info=False;User ID=MariaKilsved;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;