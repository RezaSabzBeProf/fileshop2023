using fileshop2023.DataLayer.entities.user;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.DataLayer.order
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int OrderSum { get; set; }

        public bool IsFinaly { get; set; } = false;

        public DateTime CreateDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
