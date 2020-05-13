using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderedProducts> OrderedProducts { get; set; }
    }
}
