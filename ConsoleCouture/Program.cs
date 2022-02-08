using System;

namespace ConsoleCouture
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═╗┌─┐┌┐┌┌─┐┌─┐┬  ┌─┐  ╔═╗┌─┐┬ ┬┌┬┐┬ ┬┬─┐┌─┐");
            Console.WriteLine("║  │ ││││└─┐│ ││  ├┤   ║  │ ││ │ │ │ │├┬┘├┤");
            Console.WriteLine("╚═╝└─┘┘└┘└─┘└─┘┴─┘└─┘  ╚═╝└─┘└─┘ ┴ └─┘┴└─└─┘");
            Console.ResetColor();
            Console.WriteLine();


        }
    }
}


//Server=tcp:mariak.database.windows.net,1433;Initial Catalog=ConsoleCouture;Persist Security Info=False;User ID=MariaKilsved;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;