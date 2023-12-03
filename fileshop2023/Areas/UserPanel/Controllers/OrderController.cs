using Dto.Payment;
using fileshop2023.Core;
using fileshop2023.Core.Service;
using fileshop2023.Core.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZarinPal.Class;

namespace fileshop2023.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]

    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Route("Cart")]
        public IActionResult Cart()
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var error = _orderService.UpdateCart(userid);
            var order = _orderService.GetUserCart(userid);

            if(error == Core.DTO.CartErrorViewModel.Empty)
            {
                ViewBag.error = "empty";
            }
            return View(order);
        }

        public IActionResult AddCart(int id)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _orderService.AddCart(userid,id);
            return RedirectToAction(nameof(Cart));
        }
        public async Task<IActionResult> StartPayment()
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var error = _orderService.UpdateCart(userid);
            var order = _orderService.GetUserCart(userid);

            if (error == Core.DTO.CartErrorViewModel.Empty)
            {
                return RedirectToAction(nameof(Cart));
            }

            //new payment
            var expose = new Expose();
            Payment _payment;
            Authority _authority;
            Transactions _transactions;
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
            var result = await _payment.Request(new DtoRequest()
            {
                Mobile = order.User.Phone,
                CallbackUrl = $"https://localhost:44304/VrPayment/" + order.OrderId,
                Description = $"پرداخت سفارش شماره {order.OrderId}",
                Email = "test@gmail.com",
                Amount = order.OrderSum,
                MerchantId = "merchent"
            }, ZarinPal.Class.Payment.Mode.zarinpal);
            //end new payment

            return Redirect("https://zarinpal.com/pg/StartPay/" + result.Authority);
        }
        [Route("VrPayment")]
        public async Task<IActionResult> VrPayment(int id)
        {
            ViewBag.success = false;
            if (HttpContext.Request.Query["Status"] != "" &&
              HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
              && HttpContext.Request.Query["Authority"] != "")
            {

                var order = _orderService.GetOrderById(id);
                string authority = HttpContext.Request.Query["Authority"];


                ZarinPalVerify zarin = new ZarinPalVerify();
                var resp = zarin.VertifyZarinPal(authority, order.OrderSum, "merchent");


                if (resp.data.code == 100)
                {
                    bool x = _orderService.FinalizeOrder(id);
                    if(x)
                    {
                        ViewBag.success = true;

                        return View();
                    }
                    else
                    {
                        return View();
                    }

                }
                else
                {
                    return View();

                }
            }
            return View();
        }
        public IActionResult UserProductList()
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            var model = _orderService.GetUserProducts(userid);
            return View(model);
        }
        public IActionResult DownloadFile(int id)
        {
            int userid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (_orderService.IsUserBuyProduct(userid,id))
            {
                // اجازه دانلود دارد
                var product = _orderService.GetProductById(id);

                string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/files", product.FileName);
              
                
                
                byte[] file = System.IO.File.ReadAllBytes(path);

                return File(file, "application/force-download", product.FileName);
            }
            else
            {
                // اجازه ندارد
                return Unauthorized();
            }
        }
    }
}
