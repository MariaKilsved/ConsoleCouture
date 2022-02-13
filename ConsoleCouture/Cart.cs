using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture
{
    class Cart
    {

        protected List<Models.CartItemQuery> cartList = new();

        public Models.CartItemQuery this[int idx]
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

        public override string ToString()
        {
            string sRet = "---CART---:\n";

            //Using Linq but no query
            var groupedCart = (from x in cartList
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Item = g.Key, Count = count }).ToList();


            foreach (var x in groupedCart)
            {
                sRet += $"{x.Item.ProductId,-5}{x.Item.ProductName} ({x.Item.Size})\n{x.Item.Price:C2}{x.Count, -20}\n\n";
            }

            decimal sum = 0;
            foreach(var product in cartList)
            {
                sum += (product.Price ?? 0);
            }

            sRet += $"Summa: {sum:C2}";
            return sRet;
        }

        //TO DO: Check what is already in cart to make sure you don't add more than what's in stock
        public void Add(int id)
        {
            using (var db = new Models.ConsoleCoutureContext())
            {
                var products = from product in db.Products
                               join
                               stock in db.Stocks on product.Id equals stock.ProductId
                               where product.Id == id && stock.InStock > 0
                               select new Models.CartItemQuery { ProductId = product.Id, ProductName = product.Name, Price = product.Price, StockId = stock.Id, Size = stock.Size, InStock = stock.InStock };
                
                cartList.Add(products.ToList()[0]);
            }
        }

        //TO DO: Error handling
        public void Remove(int id)
        {
            for(int i = 0; i < cartList.Count; i++)
            {
                if(cartList[i].ProductId == id)
                {
                    cartList.RemoveAt(i);
                }
            }
        }

        public int CountItemOccurences(int stockId)
        {
            int count = 0;

            foreach(var item in cartList)
            {
                if(item.StockId == stockId)
                {
                    count++;
                }
            }
            return count;
        }

        public void IncreaseQuantity(int stockId)
        {
            int count = CountItemOccurences(stockId);
            var tempProduct = new Models.CartItemQuery();

            for (int i = 0; i < cartList.Count; i++)
            {
                if (cartList[i].StockId == stockId)
                {
                    tempProduct = cartList[i];
                    break;
                }
            }


//-----------------------TODO

            using (var db = new Models.ConsoleCoutureContext())
            {
                var availableUnits = from stock in db.Stocks
                            where stock.Id == stockId && stock.InStock > 0
                            select stock.InStock;

            }
        }
    }
}
