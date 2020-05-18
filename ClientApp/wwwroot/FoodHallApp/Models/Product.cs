using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodHallApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
