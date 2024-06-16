using CreativePhotography.Models;

namespace CreativePhotography.IService
{
    public interface IMailService
    {
        Task<string> SendMailToAdmin(ContactUsModel userInfo);
    }
}
