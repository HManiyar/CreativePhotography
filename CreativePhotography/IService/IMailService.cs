using PixelCreator.Models;

namespace PixelCreator.IService
{
    public interface IMailService
    {
        Task<string> SendMailToAdmin(ContactUsModel userInfo);
    }
}
