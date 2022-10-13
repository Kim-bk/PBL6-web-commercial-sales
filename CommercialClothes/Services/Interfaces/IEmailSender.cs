using System.Threading.Tasks;

namespace CommercialClothes.Services
{
    public interface IEmailSender
    {
        public Task SendEmailVerificationAsync(string toEmail, string code, string emailFor);
    }
}