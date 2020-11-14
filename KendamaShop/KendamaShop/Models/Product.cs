using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KendamaShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        // public int SellerId { get; set; }

        // [Required]
        // public int CategoryId { get; set; }

        // [ForeignKey("CategoryId")]
        // public virtual Category Category { get; set; }

        [StringLength(30)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Title field is required")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price field is required")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Rating field is required")]
        public float Rating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}