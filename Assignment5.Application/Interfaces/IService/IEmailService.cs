using Assignment7.Domain.Models.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment7.Application.Interfaces.IService
{
    public interface IEmailService
    {
        bool SendMail(MailData mailData);
    }
}
