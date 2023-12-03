using fileshop2023.Core.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace fileshop2023.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult ShowProduct(int id)
        {
            return View(_productService.GetProductForShow(id));
        }
        public IActionResult Products()
        {
            return View();
        }
    }
}
