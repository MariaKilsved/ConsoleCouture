using System;

namespace ConsoleCouture
{
    class Program
    {
        static void Main(string[] args)
        {
            int menuSelection;
            int productId = 0;
            int sizeId = 0;
            bool Continue;
            string title = "Skriv in Id:et för en produkt för mer information";
            string title2 = "Skriv in id:et för en produkt för att lägga till den till kundvagnen";
            Cart cart = new Cart();

            do
            {
                Continue = MainMenu(out menuSelection);
                if (!Continue) return;

                switch (menuSelection)
                {
                    case 1:
                        ProductView.ListAllProducts();
                        Continue = ProductView.SelectProduct(title, out productId);
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
                    default:
                        break;
                }

                if(sizeId > 0)
                {
                    cart.Add(sizeId);
                }

            } while (Continue);

            if (!Continue) return;
        }

        private static bool MainMenu(out int selection)
        {
            selection = 0;
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
                Console.WriteLine("[6] Administration");
                Console.WriteLine("[Q] Avsluta");

                sInput = Console.ReadLine();

                if(int.TryParse(sInput, out selection) && selection > 0 && selection < 7)
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