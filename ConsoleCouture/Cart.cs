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

        //To do: Remove doubles and include quantity
        public override string ToString()
        {
            string sRet = "---CART---:\n";

            for (int i = 0; i < cartList.Count; i++)
            {
                sRet += $"{cartList[i].ProductId,-5}{cartList[i].ProductName} ({cartList[i].Size})\n{cartList[i].Price:C2}\n\n";
            }
            return sRet;

        }

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

    }
}
