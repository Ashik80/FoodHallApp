using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodHallApp.Models.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public decimal Price { get; set; }
        public IFormFile Photo { get; set; }
    }
}
