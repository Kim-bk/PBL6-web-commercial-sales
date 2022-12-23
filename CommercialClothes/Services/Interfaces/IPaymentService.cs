using CommercialClothes.Models.DTOs.Requests;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> VNPayCheckOut(OrderRequest request, int userId);

        public Task<bool> StripeCheckOut(OrderRequest request, int userId);
        public Task<bool> VNPaySuccess(string orderInfo);
    }
}