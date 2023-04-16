using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataAppHR.Data;
using BataAppHR.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.IO;

namespace BataAppHR.Services
{
    public class MailService: IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder();
            if (request.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in request.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendVerifyEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\MailDocument.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[token]", request.callbackurl);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder();
            //byte[] fileBytes;

            //var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot");
            //var files = Path.Combine(fileUrl, "FileCodeofConduct.pdf");
         
            //builder.Attachments.Add(files);

            //using (var stream = new FileStream(files, FileMode.Create))
            //{

            //    using (var ms = new MemoryStream())
            //    {
            //        stream.CopyTo(ms);
            //        fileBytes = ms.ToArray();
            //    }
            //}

            //builder.Attachments.Add(request.Attachments[0].FileName, fileBytes, ContentType.Parse(request.Attachments[0].ContentType));
            //if (request.Attachments != null)
            //{
            //    foreach (var file in request.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {

            //        }
            //    }
            //}
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendInvoiceEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\InvoicePage.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            string orderDtl = "";
            if(request.salesorderTbl != null)
            {
                var data = request.salesorderTbl;
                orderDtl += "Invoice: " + data.picking_no + "<br />";
                orderDtl += "Total qty: " + data.TOTAL_QTY.ToString() + "<br />";
                string totalorder = "";
                decimal? totalamt = 0;
                var totalorders = data.TOTAL_ORDER;
                totalorders = totalorders - request.creditnoteval;
                totalorders = totalorders - request.custdisc;
                if (data.IS_DISC_PERC == "1")
                {
                    decimal? disc = 0;

                    if (data.INV_DISC != null)
                    {
                        disc = data.INV_DISC;

                    }
                    decimal? totaldisc = (totalorders * disc) / 100;
                    totalorder = (totalorders - totaldisc).ToString(); 
                }
                else
                {
                    decimal? disc = 0;
                    if (data.INV_DISC_AMT != null)
                    {
                        disc = data.INV_DISC_AMT;

                    }
                    totalorder = (totalorders - disc).ToString();
                }
                orderDtl += "Total Order: " + totalorder + "<br />";
                orderDtl += "Artikel: " + "<br />";
                int i = 1;
                foreach (var dt in data.salesOrderDtl)
                {
                    orderDtl += i.ToString() + " - item: " + dt.article + " - "+"qty: " + dt.qty +  " price: "+ dt.final_price+"<br />";
                    i++;
                }
            }
            MailText = MailText.Replace("[username]", request.UserName).Replace("[DetailTransaksi]", orderDtl);
        
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Transaction Recap {request.UserName}";
            var builder = new BodyBuilder();
            //byte[] fileBytes;

            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Invoice");
            var files = Path.Combine(fileUrl, request.fileinvoice);

            builder.Attachments.Add(files);

            //using (var stream = new FileStream(files, FileMode.Create))
            //{

            //    using (var ms = new MemoryStream())
            //    {
            //        stream.CopyTo(ms);
            //        fileBytes = ms.ToArray();
            //    }
            //}

            //builder.Attachments.Add(request.Attachments[0].FileName, fileBytes, ContentType.Parse(request.Attachments[0].ContentType));
            //if (request.Attachments != null)
            //{
            //    foreach (var file in request.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {

            //        }
            //    }
            //}
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendOrderConfirm(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\OrderConfirm.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            string orderDtl = "";
            if (request.salesorderTbl != null)
            {
                var data = request.salesorderTbl;
                orderDtl += "Order No: " + data.id + "<br />";
                orderDtl += "Total qty: " + data.TOTAL_QTY.ToString() + "<br />";
                string totalorder = "";
                decimal? totalamt = 0;
                var totalorders = data.TOTAL_ORDER;
                totalorders = totalorders - request.creditnoteval;
                totalorders = totalorders - request.custdisc;
                if (data.IS_DISC_PERC == "1")
                {
                    decimal? disc = 0;

                    if (data.INV_DISC != null)
                    {
                        disc = data.INV_DISC;

                    }
                    decimal? totaldisc = (totalorders * disc) / 100;
                    totalorders = totalorders - totaldisc;
                    totalorder = totalorders.ToString();
                }
                else
                {
                    decimal? disc = 0;
                    if (data.INV_DISC_AMT != null)
                    {
                        disc = data.INV_DISC_AMT;

                    }
                    totalorder = (totalorders - disc).ToString();
                }
                orderDtl += "Total Order: " + totalorder + "<br />";
                orderDtl += "Artikel: " + "<br />";
                int i = 1;
                foreach (var dt in data.salesOrderDtl)
                {
                    orderDtl += i.ToString() + " - item: " + dt.article + " - " + "qty: " + dt.qty + " price: " + dt.final_price + "<br />";
                    i++;
                }
            }
            MailText = MailText.Replace("[username]", request.UserName).Replace("[DetailTransaksi]", orderDtl);

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Transaction Recap {request.UserName}";
            var builder = new BodyBuilder();
            //byte[] fileBytes;

            var fileUrl = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\OrderConfirm");
            var files = Path.Combine(fileUrl, request.fileinvoice);

            builder.Attachments.Add(files);

            //using (var stream = new FileStream(files, FileMode.Create))
            //{

            //    using (var ms = new MemoryStream())
            //    {
            //        stream.CopyTo(ms);
            //        fileBytes = ms.ToArray();
            //    }
            //}

            //builder.Attachments.Add(request.Attachments[0].FileName, fileBytes, ContentType.Parse(request.Attachments[0].ContentType));
            //if (request.Attachments != null)
            //{
            //    foreach (var file in request.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {

            //        }
            //    }
            //}
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


    }
}
