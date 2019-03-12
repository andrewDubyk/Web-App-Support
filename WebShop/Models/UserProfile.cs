using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.Products = new List<Product>();
        }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ImageName { get; set; }
        public DateTime DateRegistered { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
