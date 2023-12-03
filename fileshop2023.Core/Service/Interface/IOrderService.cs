using fileshop2023.Core.DTO;
using fileshop2023.DataLayer.entities.product;
using fileshop2023.DataLayer.entities.user;
using fileshop2023.DataLayer.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.Core.Service.Interface
{
    public interface IOrderService
    {
        CartErrorViewModel UpdateCart(int userid);

        void AddCart(int userid, int productid);

        Order GetUserCart(int userid);

        Order GetOrderById(int orderid);

        bool FinalizeOrder(int orderid);

        IEnumerable<UserProduct> GetUserProducts(int userid);

        bool IsUserBuyProduct(int userid,int productid);

        Product GetProductById(int productid);
    }
}
