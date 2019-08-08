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
        Functions function = new Functions();
        public string getNotifications(int ProfileID)
        {
            string notificationDisplayString = "No Notifications";
            List<Model.Notifications> notifications = new List<Model.Notifications>();

            try
            {
                Notifications getNoti = new Notifications();
                getNoti.ProfileID = ProfileID;
                notifications = handler.getNotifications(getNoti);
            }
            catch(Exception Err)
            {
                function.logAnError("Error Loading notifictaions " +Err);
                notificationDisplayString = "Error Loading Notifications";
            }


            if(notifications.Count > 0)
            {
                notificationDisplayString = "";

                foreach (Model.Notifications noti in notifications)
                {
                    notificationDisplayString += " <a class='dropdown-item' href='../Notifications/dismissNotifications?ID=" + noti.notificationID + "'>"
                                            + noti.Details + "</a> -"
                                     + noti.timeSince + " days since.";
                }
            }

            return notificationDisplayString;
        }

        public string dismissNotification(int getNotiID)
        {
            string link = "../Shared/Error?Err=An error occurred while loading Notification.";
            Notifications tryLink = null;
            Notifications notiID = new Notifications();
            notiID.notificationID = getNotiID;

            try
            {
                tryLink = handler.dismissNotifications(notiID);
            }
            catch (Exception Err)
            {
                function.logAnError("Error Dissmissing notifictaion ID: "+notiID.notificationID+" Details:" + Err);
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
                function.logAnError("Error creating new notifictaion"+ " Details:" + Err);
            }

            if(result == false)
                function.logAnError("Error creating new notifictaion");

            return result;
        }
    }
}
