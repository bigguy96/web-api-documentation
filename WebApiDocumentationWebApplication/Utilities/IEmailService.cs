using System.Threading.Tasks;

namespace WebApiDocumentationWebApplication.Utilities
{
    public interface IEmailService
    {
        Task SendAsync(string email, string name, string subject, string body);
    }
}