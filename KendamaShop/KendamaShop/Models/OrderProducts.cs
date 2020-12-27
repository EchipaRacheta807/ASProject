using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KendamaShop.Models
{
    public class OrderProducts
    {
        public OrderProducts()
        {

        }

        public OrderProducts(int _OrderId, int _ProductId, int _quantity)
        {
            OrderId = _OrderId;
            ProductId = _ProductId;
            Quantity = _quantity;
        }

        [Key, Column(Order = 0)]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}