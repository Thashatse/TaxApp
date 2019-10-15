using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class NotificationsFunctions
    {
        IDBHandler handler = new DBHandler();
        public List<Notifications> getNotifications(int ProfileID)
        {
            List<Model.Notifications> notifications = new List<Model.Notifications>();

            try
            {
                Notifications getNoti = new Notifications();
                getNoti.ProfileID = ProfileID;
                notifications = handler.getNotifications(getNoti);
                notifications = notifications.OrderBy(x => x.timeSince).ToList();
            }
            catch(Exception Err)
            {
                logAnError("Error Loading notifictaions " +Err);
                Notifications errorNoti = new Notifications();
                errorNoti.notificationID = 0;
                errorNoti.Details = "Error Loading Notifications";
                notifications.Add(errorNoti);
            }


            if(notifications.Count <= 0)
            {
                Notifications noNoti = new Notifications();
                noNoti.notificationID = 0;
                noNoti.Details = "No Notifications";
                notifications.Add(noNoti);
            }

            return notifications;
        }

        public string dismissNotification(int getNotiID, string redirect)
        {
            string link = "../Shared/Error?Err=An error occurred while loading Notification.";
            Notifications tryLink = null;
            Notifications notiID = new Notifications();
            notiID.notificationID = getNotiID;

            try
            {
                if(redirect == "Redirect")
                    tryLink = handler.getNotificationLink(notiID);
                else
                    tryLink = handler.dismissNotifications(notiID);
            }
            catch (Exception Err)
            {
                logAnError("Error Dissmissing notifictaion ID: "+notiID.notificationID+" Details:" + Err);
            }


            if (tryLink != null && tryLink.Link !="")
            {
                link = tryLink.Link;
            }

            return link;
        }

        public bool newNotification(Notifications newNotification)
        {
            bool result = false;

            try
            {
                result = handler.newNotification(newNotification);
            }
            catch (Exception Err)
            {
                logAnError("Error creating new notifictaion"+ " Details:" + Err);
            }

            if(result == false)
                logAnError("Error creating new notifictaion");

            return result;
        }

        public void OutstandingInvoiceReminders()
        {
            try
            {
                List<OutstandingInvoiceReminders> OIRs = handler.getOverdueInvoices();

                if (OIRs.Count > 0)
                {
                    foreach (OutstandingInvoiceReminders OIR in OIRs)
                    {
                        if (OIR != null && OIR.InvoiceNum != null)
                        {
                            Notifications newNoti = new Notifications();
                            newNoti.date = OIR.DateTime;
                            newNoti.ProfileID = OIR.ProfileID;
                            //Change befor Publishing
                            //newNoti.Link = "../Invoice/Invoice?id=" + OIR.InvoiceNum;
                            newNoti.Link = "http://sict-iis.nmmu.ac.za/taxapp/Invoice/Invoice?id=" + OIR.InvoiceNum;
                            newNoti.Details = OIR.ClientName + " has an outstanding invoice for job " + OIR.JobTitle + ".";

                            bool create = true;
                            List<Notifications> ProfileNotis = getNotifications(OIR.ProfileID);
                            foreach (Notifications noti in ProfileNotis)
                            {
                                if (noti.Details == newNoti.Details
                                    && noti.Link == newNoti.Link)
                                    create = false;
                            }

                            if (OIR.DaysSince == 4 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 12 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 19 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 26 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 33 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 60 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 90 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 180 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 270 && create)
                                newNotification(newNoti);
                            if (OIR.DaysSince == 360 && create)
                                newNotification(newNoti);
                        }
                        else
                        {
                            logAnError("Error loading outstanding invoice reminders ");
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                logAnError("Error loading outstanding invoice reminders " + " Details:" + Err);
            }
        }
        
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
    }
}
