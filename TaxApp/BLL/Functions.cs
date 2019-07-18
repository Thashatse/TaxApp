using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Functions
    {
        IDBHandler handler = new DBHandler();
        public void logAnError(string Err)
        {
            /*
            * Logs Error Details in a text File
            */
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"" + AppDomain.CurrentDomain.BaseDirectory + "TaxAppErrorLog.txt", true))
                {
                    file.WriteLine();
                    file.WriteLine("TimeStamp: " + DateTime.Now);
                    file.WriteLine("Machine Name: " + Environment.MachineName);
                    file.WriteLine("OS Version: " + Environment.OSVersion);
                    file.WriteLine("Curent User: " + Environment.UserName);
                    file.WriteLine("User Domain: " + Environment.UserDomainName);
                    file.WriteLine("Curent Directory: " + Environment.CurrentDirectory);
                    file.WriteLine("Error: ");
                    file.WriteLine(Err);
                }
            }
            catch
            {

            }
        }

        public string generateNewInvoiceNum()
        {
            try
            {
                string invoiceNum = DateTime.Now.ToString("ddMMyyyy");

                int count = handler.getInvoiceTodaysCount();

                if (count == -1)
                {
                    logAnError("Error generating invoice number");
                    return null;
                }
                else if(count < 10)
                {
                    count += 1;
                    invoiceNum += "00" + count;
                }
                else if(count < 100)
                {
                    count += 1;
                    invoiceNum += "0" + count;
                }
                else
                {
                    count += 1;
                    invoiceNum += count;
                }

                return invoiceNum;
            }
            catch
            {
                logAnError("Error generating invoice number");
            }
            return null;
        }

        public bool sendEmail(string receverAddress, string reciverName, string subject, string body, string senderName, int ProfileID)
        {
            bool success = false;
            try
            {
                Model.EmailSetting getSettings = new Model.EmailSetting();
                getSettings.ProfileID = ProfileID;
                Model.EmailSetting settings = handler.getEmailSettings(getSettings);

                if(settings != null)
                {
                    if (settings.Address == null)
                    {
                        getSettings = new Model.EmailSetting();
                        getSettings.ProfileID = 0;
                        settings = handler.getEmailSettings(getSettings);
                    }
                }
                else
                {
                    getSettings = new Model.EmailSetting();
                    getSettings.ProfileID = 0;
                    settings = handler.getEmailSettings(getSettings);
                }

                var fromAddress = new MailAddress(settings.Address, senderName);
                var toAddress = new MailAddress(receverAddress, reciverName);
                string fromPassword = settings.Password.ToString();

                var smtp = new SmtpClient
                {
                    Host = settings.Host,
                    Port = int.Parse(settings.Port),
                    EnableSsl = settings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = settings.UseDefailtCredentials,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                success = true;
            }
            catch (Exception err)
            {
                logAnError("Error sending email To: " + receverAddress
                    + "Subject: " + subject
                    + " Error:" + err);
                success = false;
            }
            return success;
        }

    }
}
