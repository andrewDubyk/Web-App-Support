using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Product
    {
        public Product()
        {
            this.ProductInCarts = new List<ProductInCart>();
        }

        [Key]
        public string ProductId { get; set; }

        [ForeignKey("UserProfile")]
        public string UserProfileId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public double Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<ProductInCart> ProductInCarts { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
