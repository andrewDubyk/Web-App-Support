using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class ProductInCart
    {
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
