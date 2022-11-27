using CommercialClothes.Models.DTOs.Requests;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> SendPayment(OrderRequest request, int userId);
    }
}
