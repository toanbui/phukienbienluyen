using Entities.Param.FrontEnd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Utilities;

namespace Utilities.Helper
{
    public static class EmailHelper
    {
        //public static void SendMail()
        //{
        //    string body = createEmailBody("Thủy lê", "Please check your account Information", "Anh chào em");

        //    SendHtmlFormattedEmail("Phụ kiện hoàng gia - Đặt hàng", body);
        //}
        public static void SendOrderMail(CheckOutModel model , string toEmail)
        {
            string ProductFormat = @"<tr class=""product""> <td class=""product-image""> <div class=""product-thumbnail"" style=""width: 4.6em; height: 4.6em; border-radius: 8px; background: #fff; position: relative;""> <div class=""product-thumbnail-wrapper"" style=""width: 100%; height: 100%; position: relative; overflow: hidden; border-radius: 8px;""> <img class=""product-thumbnail-image"" style=""width: 100%; position: absolute; top: 0; left: 0; right: 0; bottom: 0; max-width: 100%; max-height: 100%; margin: auto;"" alt=""{0}"" src=""{1}""> </div> </div> </td> <td class=""product-description""> <span class=""product-description-name order-summary-emphasis"">{2}</span> </td> <td class=""product-quantity visually-hidden"" style=""text-align:center"">{3}</td> <td class=""product-price""> <span class=""order-summary-emphasis"">{4}</span> </td><td class=""product-price""> <span class=""order-summary-emphasis"">{5}</span> </td> </tr>";
            StringBuilder sbProduct = new StringBuilder();
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("/configs/OrderTemplate.html")))

            {

                body = reader.ReadToEnd();

            }

            if (model.ProductParam.ProductInCarts != null && model.ProductParam.ProductInCarts.Any())
            {
                foreach (var item in model.ProductParam.ProductInCarts)
                {
                    item.TotalPrice = (long)(item.Number * item.Price);
                    sbProduct.AppendFormat(ProductFormat,
                        item.Name.EncodeTitle(),
                        Config.DOMAIN + item.FirstAvatar.ChangeThumbSize(100, 100),
                        item.Name,
                        item.Number,
                        item.Price?.ToCurrency(),
                        item.TotalPrice.ToCurrency()
                    );
                }
            }

            body = body.Replace("{{Product}}", sbProduct.ToString()); //replacing the required things  
            body = body.Replace("{{CustName}}", model.CartOrder.Name);
            body = body.Replace("{{CustPhone}}", model.CartOrder.Phone);
            body = body.Replace("{{CustEmail}}", model.CartOrder.Email);
            body = body.Replace("{{TotalPrice}}", model.ProductParam.TotalPriceInCart.ToCurrency());

            //body = body.Replace("{Title}", title);

            //body = body.Replace("{message}", message);

            SendHtmlFormattedEmail(model.CartOrder.Name + " - Đặt hàng | Case Hoàng Gia", body , toEmail);

        }

        private static void SendHtmlFormattedEmail(string subject, string body , string toEmail)
        {
            using (MailMessage mailMessage = new MailMessage())

            {

                mailMessage.From = new MailAddress("kinlero@gmail.com");

                mailMessage.Subject = subject;

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(toEmail));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";

                smtp.EnableSsl = true;

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = "kinlero@gmail.com"; //reading from web.config  

                NetworkCred.Password = "buitoan188092"; //reading from web.config  

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = 25; //reading from web.config  

                smtp.Send(mailMessage);

            }

        }
    }
}
