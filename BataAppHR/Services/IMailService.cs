using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataAppHR.Data;
using BataAppHR.Models;
namespace BataAppHR.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
        Task SendVerifyEmailAsync(WelcomeRequest request);
        
    }
}
