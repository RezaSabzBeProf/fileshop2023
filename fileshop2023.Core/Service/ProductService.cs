using fileshop2023.Core.Service.Interface;
using fileshop2023.DataLayer.context;
using fileshop2023.DataLayer.entities.product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fileshop2023.Core.Service
{
    public class ProductService : IProductService
    {
        FileShopContext _context;

        public ProductService(FileShopContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product model, IFormFile file, IFormFile img)
        {
            model.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(img.FileName);
            string imgPath = Path.Combine("wwwroot/img", model.ImageName);
            using(var stream = new FileStream(imgPath,FileMode.Create))
            {
                img.CopyTo(stream);
            }

            model.FileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
            string filePath = Path.Combine("wwwroot/files", model.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            _context.Add(model);
            _context.SaveChanges();
        }

        public void CreateProductGroup(ProductGroup productGroup)
        {
            _context.Update(productGroup);
            _context.SaveChanges();
        }

        public void EditProduct(Product product, IFormFile file, IFormFile img)
        {
            if(file != null)
            {
                string lastpath = Path.Combine("wwwroot/files", product.FileName);
                File.Delete(lastpath);


                product.FileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                string filePath = Path.Combine("wwwroot/files", product.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            if(img != null)
            {
                string lastpath = Path.Combine("wwwroot/img", product.ImageName);
                File.Delete(lastpath);



                product.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(img.FileName);
                string imgPath = Path.Combine("wwwroot/img", product.ImageName);
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    img.CopyTo(stream);
                }
            }
            _context.Update(product);
            _context.SaveChanges();
        }

        public ProductGroup GetGroup(int id)
        {
            return _context.ProductGroups.Find(id);
        }

        public List<SelectListItem> GetGroupsForProduct()
        {
            return _context.ProductGroups.Select(c => new SelectListItem
            {
                Value = c.ProductGroupId.ToString(),
                Text = c.GroupName
            }).ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public Product GetProductForShow(int productId)
        {
            return _context.Products.Include(c=> c.ProductGroup).SingleOrDefault(c => c.ProductId == productId);
        }

        public IEnumerable<ProductGroup> GetProductGroupsForAdmin()
        {
            return _context.ProductGroups;

        }

        public IEnumerable<Product> GetProductsForAdmin()
        {
            return _context.Products;
        }

        public void SoftDeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            product.IsDelete = true;
            _context.SaveChanges();
        }
    }
}
