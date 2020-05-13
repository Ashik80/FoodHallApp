using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodHallApp.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required, StringLength(20)]
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
