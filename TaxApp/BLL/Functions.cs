using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        #region Auto Functions
        public void runAutoFunctions(object profileID)
        {
            repeatExpense();

            budgetCheck(int.Parse(profileID.ToString()));
        }
        public void budgetCheck(int PID)
        {
            NotificationsFunctions notiFunctions = new NotificationsFunctions();

            Profile profileID = new Profile();
            profileID.ProfileID = PID;
            List<SP_GetJob_Result> jobs = handler.getProfileJobs(profileID);

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
                                handler.UpdateGeneralExpenseRepeate(newExpense);
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
