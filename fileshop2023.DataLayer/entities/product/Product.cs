using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.DataLayer.entities.product
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string ShortDesc { get; set; }

        public string Body { get; set; }

        public string FileName { get; set; }

        public string ImageName { get; set; }

        public int Price { get; set; }

        public bool IsDelete { get; set; } = false;
        public int ProductGroupId { get; set; }

        [ForeignKey("ProductGroupId")]
        public ProductGroup ProductGroup { get; set; }
    }
}
