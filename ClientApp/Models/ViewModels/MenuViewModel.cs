using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Models.ViewModels
{
    public class MenuViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
    }
}
