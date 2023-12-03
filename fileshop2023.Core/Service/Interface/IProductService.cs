using fileshop2023.DataLayer.entities.product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace fileshop2023.Core.Service.Interface
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductsForAdmin();

        IEnumerable<ProductGroup> GetProductGroupsForAdmin();

        void CreateProductGroup(ProductGroup productGroup);

        ProductGroup GetGroup(int id);

        List<SelectListItem> GetGroupsForProduct();

        void CreateProduct(Product model, IFormFile file, IFormFile img);

        Product GetProductById(int productId);

        Product GetProductForShow(int productId);

        void EditProduct(Product product, IFormFile file, IFormFile img);

        void SoftDeleteProduct(int productId);
    }
}
