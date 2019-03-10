using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Cart
    {
        public Cart()
        {
            this.ProductInCarts = new List<ProductInCart>();
        }

        public string CartId { get; set; }

        [ForeignKey("UserProfile")]
        public string UserProfileId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<ProductInCart> ProductInCarts { get; set; }
    }

}
