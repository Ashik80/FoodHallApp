using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodHallApp.Models.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
