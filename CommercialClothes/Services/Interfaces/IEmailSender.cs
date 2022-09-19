using System.Threading.Tasks;

namespace CommercialClothes.Services
{
    public interface IEmailSender
    {
        Task SendEmailVerificationAsync(string toEmail, string activationCode, string emailFor);
    }
}