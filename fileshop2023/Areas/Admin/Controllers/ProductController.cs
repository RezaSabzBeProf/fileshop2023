using fileshop2023.Core.Service.Interface;
using fileshop2023.DataLayer.entities.product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Text.RegularExpressions;

namespace fileshop2023.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var model = _productService.GetProductsForAdmin();
            return View(model);
        }
        public IActionResult CreateProduct()
        {
            var groups = _productService.GetGroupsForProduct();
            ViewData["groups"] = new SelectList(groups, "Value", "Text");
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product model, IFormFile File, IFormFile ImageFile)
        {
            _productService.CreateProduct(model, File, ImageFile);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditProduct(int id)
        {
            var model = _productService.GetProductById(id);
            var groups = _productService.GetGroupsForProduct();
            ViewData["groups"] = new SelectList(groups, "Value", "Text", model.ProductGroupId);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditProduct(Product model, IFormFile File, IFormFile ImageFile)
        {
            _productService.EditProduct(model, File, ImageFile);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteProduct(int id)
        {
            _productService.SoftDeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
        #region ProductGroup
        public IActionResult GroupIndex()
        {
            var model = _productService.GetProductGroupsForAdmin();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateProductGroup(int id = 0)
        {
            ProductGroup model = new ProductGroup
            {
                ProductGroupId = id,
                GroupName = ""
            };
            if (id != 0)
            {
                model.GroupName = _productService.GetGroup(id).GroupName;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateProductGroup(ProductGroup group)
        {
            _productService.CreateProductGroup(group);
            return RedirectToAction(nameof(GroupIndex));
        }
        #endregion

    }
}
