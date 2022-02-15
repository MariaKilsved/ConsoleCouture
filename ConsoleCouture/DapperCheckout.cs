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
        private int? customerId;
        private int? orderId;
        private float? discount;

        public DapperCheckout(float? discount = 0f)
        {
            this.discount = discount;
        }

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

        public bool Login()
        {
            string sInput;
            Console.WriteLine("Logga in");

            var users = new List<Models.CustomerQuery>();
            while (true)
            {
                Console.WriteLine("Ange din mailadress:");
                sInput = Console.ReadLine();

                var sql = $"SELECT * FROM Customers WHERE [Mail] = {sInput}";
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    users = connection.Query<Models.CustomerQuery>(sql).ToList();

                    if(users.Count > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Användaren hittades inte. Försök igen.");
                    }
                }
            }
            Console.WriteLine();

            int attempts = 3;
            var salts = new List<string>();
            while(attempts > 0)
            {
                Console.WriteLine("Ange lösenord:");
                sInput = Console.ReadLine();

                //The customer that is selected will be the one added to the database last. That is, the final index in the list.
                //Of course, if the same email hasn't been added multiple times this won't matter.
                var sql = $"SELECT Salt FROM Salts " +
                    $"JOIN [Customers] ON Customers.SaltId = Salts.Id" +
                    $"WHERE CustomerId = {users[^1].Id}";
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    salts = connection.Query<string>(sql).ToList();

                    if (users.Count > 0)
                    {
                        customerId = users[^1].Id;
                        Console.WriteLine($"Välkommen, {users[^1].FirstName} {users[^1].LastName}!");
                        return true;
                    }
                    else
                    {
                        attempts--;
                        Console.WriteLine($"Fel lösenord. {attempts} försök återstår.");
                    }
                }
            }
            customerId = null;
            Console.WriteLine("Inloggningen misslyckades.");
            return false;
        }

        public void PlaceOrder(List<Models.CartItemQueryGroup> cartList)
        {
            bool login = Login();
            if(!login)
            {
                return;
            }

            DateTime orderDate = DateTime.Today;
            DateTime requiredDate = orderDate.AddDays(20);
            //DateTime? shippedDate = null;                 //Because the product won't be sent yet, there will be no ShippedDate.
            decimal? freight;
            string receiverName = null;
            string shipAddress = null;
            string shipPostalCode = null;
            string shipCity = null;
            string shipCountry = null;

            Console.WriteLine("Vänligen ange mottagarens adress.");

            receiverName = ObtainStringInput("Ange mottagarens namn:", 45);
            shipAddress = ObtainStringInput("Ange mottagarens gatuadress:", 100);
            shipPostalCode = ObtainStringInput("Ange mottagarens postnummer:", 20);
            shipCity = ObtainStringInput("Ange mottagarens postort:", 50);
            shipCountry = ObtainStringInput("Ange mottagarens land:", 50);

            freight = SelectDeliveryOptions().Price;
            var payment = SelectPaymentOptions();           //Nothing else to do with this...

            Console.Clear();
            Console.WriteLine("Behandlar order, vänligen vänta...");

            //Place the order
            //Orders table
            var sql = $"INSERT INTO [Orders]([CustomerId], [OrderDate], [RequiredDate], [Freight], [ReceiverName], [ShipAddress], [ShipPostalCode], [ShipCity], [ShipCountry]) VALUES ({customerId}, '{orderDate}', {requiredDate}, {freight}, '{receiverName}', '{shipAddress}', '{shipPostalCode}', '{shipCity}', '{shipCountry}')";
            int affectedRows = 0;

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    System.Threading.Thread.Sleep(7000);
                    return;
                }
            }

            //Get the order id back. It will be the latest order. A transaction would have been nice...
            sql = $"SELECT Id FROM Orders WHERE [CustomerId] = {customerId}";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                orderId = connection.Query<int>(sql).ToList()[^1];      //orderId is a field in the DapperCheckout class
            }



            //OrderDetails and Stock
            if (affectedRows > 0)
            {
                foreach(var item in cartList)
                {
                    //Update InStock in the Stock table
                    sql = $"UPDATE Stock SET InStock = InStock - {item.quantity} WHERE Id = {item.cartItem.StockId}";

                    affectedRows = 0;

                    using (var connection = new SqlConnection(connString))
                    {
                        try
                        {
                            affectedRows = connection.Execute(sql);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            System.Threading.Thread.Sleep(7000);
                            return;
                        }
                    }

                    //Add to OrderDetails

                    sql = $"INSERT INTO OrderDetails(OrderId, ProductId, UnitPrice, Quantity, Discount) VALUES ({orderId}, {item.cartItem.ProductId}, {item.cartItem.Price}, {item.quantity}, {discount})";

                    affectedRows = 0;

                    using (var connection = new SqlConnection(connString))
                    {
                        try
                        {
                            affectedRows = connection.Execute(sql);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            System.Threading.Thread.Sleep(7000);
                            return;
                        }
                    }
                }
            }
            Console.WriteLine("Order mottagen.");
            Console.WriteLine("Välkommen åter!");
        }

        public static string ObtainStringInput(string title, int maxLength)
        {
            string sInput;

            while (true)
            {
                Console.WriteLine(title);
                sInput = Console.ReadLine();

                if (sInput.Length > maxLength)
                {
                    Console.WriteLine("För lång text Försök igen.");
                }
                else if (String.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
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
