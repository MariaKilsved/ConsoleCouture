using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient; //VisualStudio recommended change to this from just using Data.SqlClient

namespace ConsoleCouture
{
    class DapperUser
    {
        static readonly string connString = "data source=.\\SQLEXPRESS; initial catalog = ConsoleCouture; persist security info = True; Integrated Security = True;";

        public static void RegisterUser()
        {
            string firstName;          //varchar(20)
            string lastName;           //varchar(20)
            string mail;               //varchar(100)
            string phone;              //varchar(20)
            string password;           //char(32)
            int? saltId;                //int
            DateTime? birthDate;        //date
            string gender;             //char(2)

            Console.Clear();
            Console.WriteLine("Mata in dina användaruppgifter.");

            firstName = ObtainStringInput("Förnamn:", 20);
            lastName = ObtainStringInput("Efternamn:", 20);
            mail = ObtainMailInput("Mailadress:", 100);
            phone = ObtainStringInput("Telefonnummer:", 20);
            password = CreatePassword(out string salt);
            birthDate = ObtainDateInput();
            gender = ObtainGenderInput();


            //Add the salt

            var sql = $"insert into Salts(Salt) values ('{salt}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Execute(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //Get the salt id back

            sql = $"SELECT Id FROM Salts WHERE Salt = '{salt}'";

            var ids = new List<int>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                ids = connection.Query<int>(sql).ToList();
            }

            //Will be the id of the last identical salt added. There won't be a problem with duplicates since Customers could have the same salt.
            saltId = ids[^1];

            //Actually add the user:
            var affectedRows = 0;

            sql = $"INSERT INTO Customers(FirstName, LastName, Mail, Phone, Password, SaltId, BirthDate, Gender) VALUES ('{firstName}', '{lastName}', '{mail}', '{phone}', '{password}', {saltId}, '{birthDate}', '{gender}')";

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    affectedRows = connection.Execute(sql);
                    if(affectedRows > 0)
                    {
                        Console.WriteLine("Användare tillagd.");
                        System.Threading.Thread.Sleep(3000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    System.Threading.Thread.Sleep(7000);

                }
            }

        }

        private static string ObtainStringInput(string title, int maxLength)
        {
            Console.WriteLine(title);
            string sInput = Console.ReadLine();

            bool isValidating = true;
            while(isValidating)
            {
                if(string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
                }
                else if(sInput.Length > maxLength)
                {
                    Console.WriteLine("För lång text. Försök igen.");
                }
                else
                {
                    isValidating = false;
                }
            }
            return sInput;
        }

        private static string ObtainMailInput(string title, int maxLength)
        {
            Console.WriteLine(title);
            string sInput = Console.ReadLine();

            bool isValidating = true;
            while (isValidating)
            {
                if (string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
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
                    isValidating = false;
                }
            }
            return sInput;
        }

        private static string ObtainPhoneInput(string title, int maxLength)
        {
            Console.WriteLine(title);
            string sInput = Console.ReadLine();

            bool isValidating = true;
            while (isValidating)
            {
                if (string.IsNullOrWhiteSpace(sInput))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
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
                    isValidating = false;
                }
            }
            return sInput;
        }

        private static string CreatePassword(out string salt)
        {
            var security = new Utility.Security();
            string pw = "";
            salt = "";

            while(pw == "")
            {
                salt = security.GenerateSalt();
                Console.WriteLine("Välj ett lösenord:");
                string temp = security.ComputeHash(Encoding.Unicode.GetBytes(Console.ReadLine()), Encoding.Unicode.GetBytes(salt));

                if (string.IsNullOrWhiteSpace(temp))
                {
                    Console.WriteLine("Fältet är obligatoriskt. Försök igen.");
                }
                else if (temp.Length > 32)
                {
                    Console.WriteLine("För långt lösenord. Försök igen.");
                }
                else
                {
                    pw = temp;
                }
            }
            return pw;
        }

        private static DateTime ObtainDateInput()
        {
            string sInput;
            DateTime date;

            while (true)
            {
                Console.WriteLine("Ange ditt födelsedatum på formen år-månad-dag. Exempel: 1995-05-01:");
                sInput = Console.ReadLine();

                try
                {
                    date = DateTime.Parse(sInput);
                    break;
                }
                catch
                {
                    Console.WriteLine("Fel format. Försök igen.");
                }
            }
            return date;
        }

        private static string ObtainGenderInput()
        {
            string sInput;

            while(true)
            {
                Console.WriteLine("Välj kön:");
                Console.WriteLine("[K] Kvinna");
                Console.WriteLine("[M] Man");
                Console.WriteLine("[N] Icke-binär");
                sInput = Console.ReadLine();

                if(sInput == "K" || sInput == "k")
                {
                    return "F";
                }
                else if(sInput == "M" || sInput == "m")
                {
                    return "M";
                }
                else if(sInput == "N" || sInput == "n")
                {
                    return "NB";
                }
                else
                {
                    Console.WriteLine("Ogilting inmatning. Försök igen.");
                    continue;
                }
            }


        }
    }
}
