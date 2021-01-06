using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendamaShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        // Seller id
        [ForeignKey("User")]
        public string PartnerId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Category Field is required")]
        public int CategoryId { get; set; }

        // [ForeignKey("CategoryId")]
        // public virtual Category Category { get; set; }

        [StringLength(60)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Title field is required")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price field is required")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Rating field is required")]

        public float Rating { get; set; }

        public DateTime Date { get; set; }

        /*[DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose image file to upload.")]*/
        public byte[] ImageFile { get; set; }

        // 0 when a partner tries to add it. There will be a separated index page with only
        // products that haven't been accepted yet, and an admin will be able to mark them as accepted
        // when an admin adds a product, it is automatically 1.
        // the default product index method will only show products with Accepted = true
        public bool Accepted { get; set; }

        public virtual Category Category { get; set; }

        public IEnumerable<SelectListItem> Categ { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<OrderProducts> OrderProducts { get; set; }
    }
}