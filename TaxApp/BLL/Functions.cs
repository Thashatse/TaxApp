using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        NotificationsFunctions notiFunctions = new NotificationsFunctions();

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

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public int generateOTP()
        {
            Random generator = new Random();
            return generator.Next(100000, 999999);
        }

        public string removeNumericalDigit(string value)
        {
            value = value.Replace("0", "");
            value = value.Replace("1", "");
            value = value.Replace("2", "");
            value = value.Replace("3", "");
            value = value.Replace("4", "");
            value = value.Replace("5", "");
            value = value.Replace("6", "");
            value = value.Replace("7", "");
            value = value.Replace("8", "");
            value = value.Replace("9", "");

            return value;
        }

        public byte[] downloadPage(string url)
        {
            Stream stream = new MemoryStream();

            try
            {
                // create the API client instance
                pdfcrowd.HtmlToPdfClient client = new pdfcrowd.HtmlToPdfClient(
                    "THashatse", "42a95938c2d30af8b7148b7f2d8720fa");

                // run the conversion and write the result to a file
                client.convertUrlToStream(url, stream);
            }
            catch (pdfcrowd.Error why)
            {
                // report the error
                System.Console.Error.WriteLine("Pdfcrowd Error: " + why);

                // handle the exception here or rethrow and handle it at a higher level
                throw;
            }

                long originalPosition = 0;

                if (stream.CanSeek)
                {
                    originalPosition = stream.Position;
                    stream.Position = 0;
                }

                try
                {
                    byte[] readBuffer = new byte[4096];

                    int totalBytesRead = 0;
                    int bytesRead;

                    while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                    {
                        totalBytesRead += bytesRead;

                        if (totalBytesRead == readBuffer.Length)
                        {
                            int nextByte = stream.ReadByte();
                            if (nextByte != -1)
                            {
                                byte[] temp = new byte[readBuffer.Length * 2];
                                Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                                Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                                readBuffer = temp;
                                totalBytesRead++;
                            }
                        }
                    }

                    byte[] buffer = readBuffer;
                    if (readBuffer.Length != totalBytesRead)
                    {
                        buffer = new byte[totalBytesRead];
                        Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                    }
                    return buffer;
                }
                finally
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = originalPosition;
                    }
                }
            }

        #region Auto Functions
        public void runAutoFunctions(object profileID)
        {
            repeatExpense();

            budgetCheck(int.Parse(profileID.ToString()));

            notiFunctions.OutstandingInvoiceReminders();
        }
        public void budgetCheck(int PID)
        {
            NotificationsFunctions notiFunctions = new NotificationsFunctions();

            Profile profileID = new Profile();
            profileID.ProfileID = PID;
            Client client = new Model.Client();
            client.ClientID = 0;
            List<SP_GetJob_Result> jobs = handler.getProfileJobs(profileID, client);

            foreach(SP_GetJob_Result job in jobs)
            {
                bool createNewNoti = false;
                Notifications newNoti = new Notifications();
                newNoti.date = DateTime.Now;
                newNoti.ProfileID =PID;
                newNoti.Link = "../Job/Job?ID=" + job.JobID;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";

                if (job.BudgetPercent > 100 && job.noti100 == false)
                {
                    createNewNoti = true;
                    newNoti.Details = job.JobTitle +" for "+job.ClientFirstName+
                        " is over budget by "+ decimal.Parse((job.BudgetPercent - 100).ToString()).ToString("#,0.00", nfi) + "%";
                    job.noti100 = true;
                }
                else if (job.BudgetPercent > 95 && job.Noti95 == false)
                {
                    createNewNoti = true;
                    newNoti.Details = job.JobTitle + " for " + job.ClientFirstName +
                        " has " + decimal.Parse((100 - job.BudgetPercent).ToString()).ToString("#,0.00", nfi) + "% budget remaining";
                    job.Noti95 = true;
                }
                else if (job.BudgetPercent > 90 && job.Noti90 == false)
                {
                    createNewNoti = true;
                    newNoti.Details = job.JobTitle + " for " + job.ClientFirstName +
                        " has " + decimal.Parse((100 - job.BudgetPercent).ToString()).ToString("#,0.00", nfi) + "% budget remaining";
                    job.Noti90 = true;
                }
                else if (job.BudgetPercent > 75 && job.Noti75 == false)
                {
                    createNewNoti = true;
                    newNoti.Details = job.JobTitle + " for " + job.ClientFirstName +
                        " has " + decimal.Parse((100 - job.BudgetPercent).ToString()).ToString("#,0.00", nfi) + "% budget remaining";
                    job.Noti75 = true;
                }

                if(createNewNoti == true)
                    createNewNoti = notiFunctions.newNotification(newNoti);

                try
                {
                    if (createNewNoti == true)
                        createNewNoti = handler.UpdateJobNotiStatus(job);
                }
                catch (Exception err)
                {
                    logAnError("error creating budget notification Job ID: " + job.JobID +" details: "+err);
                }
            }
        }
        public void repeatExpense()
        {
            try
            {
                List<SP_GetGeneralExpense_Result> expenses = handler.getRepeatGeneralExpenses();

                if (expenses != null && expenses.Count > 0)
                {
                    foreach (SP_GetGeneralExpense_Result expense in expenses)
                    {
                        if (expense != null && expense.Name != null)
                        {
                            Model.SP_GetGeneralExpense_Result newExpense = expense;

                            newExpense.Date = DateTime.Now;
                            newExpense.Repeat = true;

                            bool result = handler.newGeneralExpense(newExpense);

                            if (result == true)
                            {
                            }
                            else
                            {
                                logAnError("Error repeting general expense in functions - Expese ID: " + expense.ExpenseID);
                            }
                        }
                        else
                        {
                            logAnError("Error running repeatExpense in functions");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logAnError(e.ToString() +
                    "Error running repeatExpense in functions");
            }
        }
        #endregion
    }
}