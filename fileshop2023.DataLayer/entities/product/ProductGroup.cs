using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.DataLayer.entities.product
{
    public class ProductGroup
    {
        [Key]
        public int ProductGroupId { get; set; }

        public string GroupName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
