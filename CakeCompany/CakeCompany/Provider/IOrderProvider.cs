using CakeCompany.Models;

namespace CakeCompany.Provider
{
    public interface IOrderProvider
    {
        Order [] GetLatestOrders();
        void  UpdateOrders(Order[] orders);
    }
    
}
