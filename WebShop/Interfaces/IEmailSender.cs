using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebShop.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(List<string> emails, string subject, string message);
    }
}
