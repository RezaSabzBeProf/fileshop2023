using fileshop2023.Core.Service.Interface;
using fileshop2023.DataLayer.entities.user;
using Kavenegar;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fileshop2023.Controllers
{
    public class UserController : Controller
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string phone)
        {
            User user = _userService.GetUserByPhone(phone);

            if (user != null)
            {
                try
                {

                    _userService.ChangeActiveCode(phone);
                    user = _userService.GetUserByPhone(phone);
                    var sender = "1000596446";
                    var receptor = user.Phone;
                    var message = $"کد تایید شما {user.ActiveCode}";
                    var api = new KavenegarApi("42724D48596469426D3662472B7142707A447A7A726E57613630647263726B434A6F4F64686F56663851733D");

                    api.VerifyLookup(receptor, user.ActiveCode.ToString(), "verifycourse");
                    return RedirectToAction("EnterActiveCode", new
                    {
                        phone = phone
                    });
                }
                catch (Kavenegar.Core.Exceptions.ApiException ex)
                {
                    // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                    Console.Write("Message : " + ex.Message);
                }
                catch (Kavenegar.Core.Exceptions.HttpException ex)
                {
                    // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                    Console.Write("Message : " + ex.Message);
                }
            }
            else
            {
                // register
                //sms

                Random rd = new Random();
                int userId = _userService.AddUser(new User
                {
                    Money = 0,
                    ActiveCode = int.Parse(rd.Next(0, 100000).ToString("D5")),
                    Name = "",
                    Phone = phone,
                });
                user = _userService.GetUserByPhone(phone);
                try
                {
                    var sender = "1000596446";
                    var receptor = user.Phone;
                    var message = $"کد تایید شما {user.ActiveCode}";
                    var api = new KavenegarApi("42724D48596469426D3662472B7142707A447A7A726E57613630647263726B434A6F4F64686F56663851733D");
                    api.VerifyLookup(receptor, user.ActiveCode.ToString(), "verifycourse");
                }
                catch (Kavenegar.Core.Exceptions.ApiException ex)
                {
                    // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                    Console.Write("Message : " + ex.Message);
                }
                catch (Kavenegar.Core.Exceptions.HttpException ex)
                {
                    // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                    Console.Write("Message : " + ex.Message);
                }

            }
            return View();
        }
        [Route("EnterActiveCode")]
        public IActionResult EnterActiveCode(string phone)
        {
            ViewBag.phone = phone;
            return View();
        }
        [HttpPost]
        [Route("EnterActiveCode")]
        public IActionResult EnterActiveCode(string phone, int activecode)
        {
            User user = _userService.GetUserByPhone(phone);
            if (user.ActiveCode == activecode)
            {
                // login
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.Name),
                        new Claim(ClaimTypes.Role,user.Role.ToString())
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                };

                HttpContext.SignInAsync(principal, properties);


                return Redirect("/");
            }


            return View();
        }
        public IActionResult FakeLogin(string phone)
        {
            User user = _userService.GetUserByPhone(phone);

            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.Name),
                        new Claim(ClaimTypes.Role,user.Role.ToString())
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMonths(1)
            };

            HttpContext.SignInAsync(principal, properties);
            return Redirect("/");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
