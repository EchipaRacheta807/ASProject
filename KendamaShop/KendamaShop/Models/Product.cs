using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KendamaShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        // public int SellerId { get; set; }

        // public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public float Rating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}