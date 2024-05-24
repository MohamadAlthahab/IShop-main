
using UserMangementService.Models;

namespace UserMangementService.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
