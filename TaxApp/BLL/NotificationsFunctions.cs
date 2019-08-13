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
        public List<Notifications> getNotifications(int ProfileID)
        {
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
