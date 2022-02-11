using System;

namespace ConsoleCouture
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═╗┌─┐┌┐┌┌─┐┌─┐┬  ┌─┐  ╔═╗┌─┐┬ ┬┌┬┐┬ ┬┬─┐┌─┐");
            Console.WriteLine("║  │ ││││└─┐│ ││  ├┤   ║  │ ││ │ │ │ │├┬┘├┤");
            Console.WriteLine("╚═╝└─┘┘└┘└─┘└─┘┴─┘└─┘  ╚═╝└─┘└─┘ ┴ └─┘┴└─└─┘");
            Console.ResetColor();

            bool Continue = MainMenu(out int selection);
            if (!Continue) return;

            switch (selection)
            {
                case 1: 
                    ProductView.ListAllProducts();
                    break;
                case 2:
                    ProductView.ListProductsByCategory();
                    break;
                case 3:
                    ProductView.ListProductsByPriceLowToHigh();
                    break;
                case 4:
                    ProductView.ListProductsByPriceHighToLow();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Sök produkt efter namn:");
                    ProductView.SearchProductsByName(Console.ReadLine());
                    break;
                default:
                    break;
            }
        }

        private static bool MainMenu(out int selection)
        {
            selection = 0;
            string sInput;
            Console.Clear();

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