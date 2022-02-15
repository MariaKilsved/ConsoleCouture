using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture
{
    class Cart
    {

        protected List<Models.CartItemQueryGroup> cartList = new();
        const decimal VAT = 0.12m;

        public Models.CartItemQueryGroup this[int idx]
        {
            get
            {
                //Basic indexer
                if (idx < 0 || idx >= cartList.Count)
                    throw new IndexOutOfRangeException("Index out of range");
                return cartList[idx];
            }
        }

        
        public void Clear()
        {
            cartList.Clear();
        }
        
        public decimal SumPrice()
        {
            decimal sum = 0;
            foreach (var product in cartList)
            {
                sum += (product.cartItem.Price ?? 0);
            }

            return sum;
        }

        public override string ToString()
        {
            string sRet = "I KUNDVAGNEN:\n";

            foreach (var x in cartList)
            {
                sRet += $"{x.cartItem.ProductId,-5}{x.cartItem.ProductName} ({x.cartItem.Size})\n{x.cartItem.Price:C2}\tAntal: {x.quantity}\n\n";
            }

            //Calculate sum
            decimal sum = SumPrice();

            sRet += $"Summa: {sum:C2}\n";
            sRet += $"Moms: {(sum * VAT):C2}\n";
            sRet += $"Totalt: {(sum * (1 + VAT)):C2}\n";
            return sRet;
        }

        public bool CartMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Mata in C för att gå till kassan.");
            Console.WriteLine("Välj M för att gå tillbaka till huvudmenyn");
            Console.WriteLine("Tryck Q för att avsluta");
            string sInput = "";

            while(true)
            {
                sInput = Console.ReadLine();

                if(String.IsNullOrWhiteSpace(sInput) || sInput == "M" || sInput == "m")
                {
                    return true;
                }
                else if(sInput == "Q" || sInput == "q")
                {
                    return false;
                }
                else if(sInput == "C" || sInput == "c")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt alternativ. Försök igen.");
                }
            }

            var delivery = DapperCheckout.SelectDeliveryOptions();
            var payment = DapperCheckout.SelectPaymentOptions();

            //Calculate sum
            decimal sumVAT = SumPrice() * (1 + VAT);

            Clear();

            Console.Clear();
            Console.WriteLine("Placeholder.");
            Console.WriteLine($"Du \"betalade\" {sumVAT:C2} för varorna.");
            Console.WriteLine("Välkommen åter!");
            Console.ReadKey();

            return true;
        }

        public void AddNewProduct(int stockId)
        {
            bool success;

            //Check to see if item already exists, and increase quantity if it does
            foreach (var item in cartList)
            {
                if(item.cartItem.StockId == stockId)
                {
                    success = IncreaseQuantity(stockId);
                    if(success)
                    {
                        Console.WriteLine($"{item.cartItem.ProductName}({item.cartItem.Size}) lades till i kundvagnen.");
                        System.Threading.Thread.Sleep(3000);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Misslyckades med att lägga till i kundvagnen. Produkten är slut på lager.");
                        System.Threading.Thread.Sleep(3000);
                        return;
                    }
                }
            }

            //Adding the item for the first time
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                               join
                               stock in db.Stocks on product.Id equals stock.ProductId
                               where stock.Id == stockId && stock.InStock > 0
                               select new Models.CartItemQuery { ProductId = product.Id, ProductName = product.Name, Price = product.Price, StockId = stock.Id, Size = stock.Size, InStock = stock.InStock };

                var singleProduct = new Models.CartItemQueryGroup { cartItem = products.ToList()[0], quantity =  1 };
                cartList.Add(singleProduct);
                Console.Clear();
                Console.WriteLine($"{products.ToList()[0].ProductName}({products.ToList()[0].Size}) lades till i kundvagnen.");
                System.Threading.Thread.Sleep(3000);
            }
        }

        public bool Remove(int stockId)
        {
            for(int i = 0; i < cartList.Count; i++)
            {
                if(cartList[i].cartItem.StockId == stockId)
                {
                    cartList[i].quantity = cartList[i].quantity - 1;

                    if(cartList[i].quantity < 1)
                    {
                        cartList.RemoveAt(i);
                    }
                    Console.WriteLine($"{cartList[i].cartItem.ProductName} ({cartList[i].cartItem.Size}) togs bort ur kundvagnen.");
                    System.Threading.Thread.Sleep(3000);
                    return true;
                }
            }
            return false;
        }

        /*
        public int CountItemOccurences(int stockId)
        {
            int count = 0;

            foreach(var item in cartList)
            {
                if(item.cartItem.StockId == stockId)
                {
                    count++;
                }
            }
            return count;
        }
        */

        public bool IncreaseQuantity(int stockId)
        {
            //Check to see if it's possible to add
            using (var db = new Models.ConsoleCoutureContext())
            {
                var availableUnits = from stock in db.Stocks
                            where stock.Id == stockId && stock.InStock > 0
                            select stock.InStock;


                //Actually add the item
                foreach (var item in availableUnits.ToList())
                {
                    foreach(var cIG in cartList)
                    {
                        if(cIG.cartItem.StockId == stockId && item > cIG.quantity)
                        {
                            cIG.quantity++;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
