using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient; //VisualStudio recommended change to this from just using Data.SqlClient

namespace ConsoleCouture
{
    class DapperCheckout
    {
        static readonly string connString = "data source=.\\SQLEXPRESS; initial catalog = ConsoleCouture; persist security info = True; Integrated Security = True;";

        public static Models.DeliveryOptionQuery SelectDeliveryOptions()
        {
            //Obtain options
            var sql = "SELECT * FROM DeliveryOptions";
            var options = new List<Models.DeliveryOptionQuery>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                options = connection.Query<Models.DeliveryOptionQuery>(sql).ToList();
            }

            //Print options
            Console.WriteLine($"{"Id",-5}{"Namn",-85}{"Pris"}");
            foreach (var option in options)
            {
                Console.WriteLine($"{option.Id,-5}{option.Name,-85}{option.Price:C2}");
            }

            Console.WriteLine();

            //Choose option
            Console.WriteLine("Mata in ett id-nummer som motsvarar det alternativ du vill ha:");
            string sInput;
            int selection;
            while(true)
            {
                sInput = Console.ReadLine();

                if (int.TryParse(sInput, out selection) && selection > 0 && selection < options.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt alternativ, försök igen.");
                }
            }

            return options[selection];
        }

        public static Models.PaymentOptionQuery SelectPaymentOptions()
        {
            //Obtain options
            var sql = "SELECT * FROM PaymentOptions";
            var options = new List<Models.PaymentOptionQuery>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                options = connection.Query<Models.PaymentOptionQuery>(sql).ToList();
            }

            //Print options
            Console.WriteLine($"{"Id",-5}{"Namn",-85}{"Pris"}");
            foreach (var option in options)
            {
                Console.WriteLine($"{option.Id,-5}{option.Name,-85}");
            }

            Console.WriteLine();

            //Choose option
            Console.WriteLine("Mata in ett id-nummer som motsvarar det alternativ du vill ha:");
            string sInput;
            int selection;
            while (true)
            {
                sInput = Console.ReadLine();

                if (int.TryParse(sInput, out selection) && selection > 0 && selection < options.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt alternativ, försök igen.");
                }
            }

            return options[selection];
        }
    }
}
