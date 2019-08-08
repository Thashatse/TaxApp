using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace TaxApp.Controllers
{
    public class NotificationsController : Controller
    {
        NotificationsFunctions notiFunctions = new NotificationsFunctions();
        public ActionResult dismissNotifications(string id = "0")
        {
            if (id == "0")
                return RedirectToAction("Error", "Shared");

            Response.Redirect(notiFunctions.dismissNotification(int.Parse(id)));

            return RedirectToAction("Error", "Shared");
        }
    }
}