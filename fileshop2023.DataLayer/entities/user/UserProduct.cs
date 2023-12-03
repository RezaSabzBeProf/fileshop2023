using fileshop2023.DataLayer.entities.product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.DataLayer.entities.user
{
    public class UserProduct
    {
        [Key]
        public int UserProductId { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
