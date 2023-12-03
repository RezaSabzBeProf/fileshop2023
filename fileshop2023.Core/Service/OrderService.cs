using fileshop2023.Core.DTO;
using fileshop2023.Core.Service.Interface;
using fileshop2023.DataLayer.context;
using fileshop2023.DataLayer.entities.user;
using fileshop2023.DataLayer.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using fileshop2023.DataLayer.entities.product;

namespace fileshop2023.Core.Service
{
    public class OrderService : IOrderService
    {
        FileShopContext _context;
        public OrderService(FileShopContext context)
        {
            _context = context;
        }

        public void AddCart(int userid, int productid)
        {
            UpdateCart(userid);
            var order = _context.Orders.SingleOrDefault(c => c.UserId == userid && c.IsFinaly == false);
            var product = _context.Products.Find(productid);
            _context.OrderDetails.Add(new DataLayer.order.OrderDetail
            {
                Amount = product.Price,
                ProductId = product.ProductId,
                OrderId = order.OrderId
            });
            _context.SaveChanges();
        }

        public bool FinalizeOrder(int orderid)
        {
            var order = _context.Orders.Include(c=> c.OrderDetails).SingleOrDefault(c=> c.OrderId == orderid);
            var user = _context.Users.SingleOrDefault(c => c.UserId == order.UserId);
            if(order.IsFinaly == false)
            {
                order.IsFinaly = true;
                foreach(var item in order.OrderDetails)
                {
                    _context.UserProducts.Add(new UserProduct
                    {
                        ProductId = item.ProductId,
                        UserId =  user.UserId,
                    });
                }
                _context.SaveChanges();
            }
            return false;
        }

        public Order GetOrderById(int orderid)
        {
            return _context.Orders.Find(orderid);
        }

        public Product GetProductById(int productid)
        {
            return _context.Products.Find(productid);
        }

        public Order GetUserCart(int userid)
        {
            var order = _context.Orders.Include(c=> c.User).Include(c=> c.OrderDetails).ThenInclude(c=> c.Product).SingleOrDefault(c => c.UserId == userid && c.IsFinaly == false);
            return order;
        }

        public IEnumerable<UserProduct> GetUserProducts(int userid)
        {
            return _context.UserProducts.Include(c=> c.Product).Where(c => c.UserId == userid);
        }

        public bool IsUserBuyProduct(int userid, int productid)
        {
            var product = _context.Products.Find(productid);
            if(product.Price == 0)
            {
                return true;
            }
            else
            {
                if(_context.UserProducts.Any(c=> c.UserId == userid && c.ProductId == productid))
                {
                    return true;
                }
            }
            return false;
        }

        public CartErrorViewModel UpdateCart(int userid)
        {
            var order = _context.Orders.Include(c=> c.OrderDetails).SingleOrDefault(c => c.UserId == userid && c.IsFinaly == false);
            if (order == null)
            {
                // create order
                _context.Orders.Add(new DataLayer.order.Order
                {
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = 0,
                    UserId = userid,
                });
                _context.SaveChanges();
                return CartErrorViewModel.Empty;
            }
            else
            {
                if(order.OrderDetails == null)
                {
                    return CartErrorViewModel.Empty;
                }
                order.OrderSum = 0;
                foreach (var item in order.OrderDetails)
                {
                    order.OrderSum += item.Amount;
                }
                _context.SaveChanges();
                return CartErrorViewModel.Success;
            }
        }
    }
}
