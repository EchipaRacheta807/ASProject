using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KendamaShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required]
        public string CategoryName { get; set; }

        public int AdminId { get; set; }

        //public ICollection<Product> Products { get; set; }
    }
}