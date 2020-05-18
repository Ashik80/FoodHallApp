using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodHallApp.Models
{
    public class OrderedProducts
    {
        public int OrderedProductsID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
