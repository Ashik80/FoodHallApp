using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required, StringLength(20)]
        public string CategoryName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
