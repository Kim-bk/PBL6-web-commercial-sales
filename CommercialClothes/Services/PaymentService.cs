using CommercialClothes.Commons.VNPay;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Web;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace CommercialClothes.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IUserRepository _userRepo;
        private readonly VNPaySettings _VNPaySettings;
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _context;
        public PaymentService(IUserRepository userRepo, IUnitOfWork unitOfWork
            , IMapperCustom mapper, IOptions<VNPaySettings> vnPay
            , IOrderService orderService, IHttpContextAccessor context) : base(unitOfWork, mapper)
        {
            _userRepo = userRepo;
            _VNPaySettings = vnPay.Value;
            _orderService = orderService;
            _context = context;
        }

        public async Task<string> SendPayment(OrderRequest request, int userId)
        {
            // 1. Add order
            var orderId = await _orderService.AddOrder(request, 6);

            // 2. Find user
            var user = await _userRepo.FindAsync(us => us.Id == userId);

            // 2. Setting variables vnPay
            string vnp_ReturnUrl = _VNPaySettings.ReturnUrl; //URL nhan ket qua tra ve 
            string vnp_Url =  _VNPaySettings.Url; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _VNPaySettings.TmnCode; //Ma website
            string vnp_HashSecret = _VNPaySettings.HashSecret; //Chuoi bi mat

            //Build URL for VNPAY
            VNPayLibrary vnpay = new();

            vnpay.AddRequestData("vnp_Version", VNPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Total * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Don hang: " + orderId);
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", orderId);

            //Billing
            vnpay.AddRequestData("vnp_Bill_Mobile", request.PhoneNumber.Trim());
            var fullName = user.Name?.Trim();
            if (!String.IsNullOrEmpty(fullName))
            {
                var indexof = fullName.IndexOf(' ');
                vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            }

            vnpay.AddRequestData("vnp_Bill_Address", request.Address.Trim());
            vnpay.AddRequestData("vnp_Bill_City", request.City.Trim());
            vnpay.AddRequestData("vnp_Bill_Country", request.Country.Trim());
            //vnpay.AddRequestData("vnp_Bill_State", "");

            // Invoice
            vnpay.AddRequestData("vnp_Inv_Phone", request.PhoneNumber.Trim());
            //vnpay.AddRequestData("vnp_Inv_Email", txt_inv_email.Text.Trim());
           // vnpay.AddRequestData("vnp_Inv_Customer", txt_inv_customer.Text.Trim());
            vnpay.AddRequestData("vnp_Inv_Address", request.Address.Trim());
            vnpay.AddRequestData("vnp_Inv_Company", request.City);
            vnpay.AddRequestData("vnp_Inv_Taxcode", request.Country);
            //vnpay.AddRequestData("vnp_Inv_Type", cbo_inv_type.SelectedItem.Value);
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
    }
}
