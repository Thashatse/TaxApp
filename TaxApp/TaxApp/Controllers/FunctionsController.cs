using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Model;
using BLL;

namespace TaxApp.Controllers
{
    public class FunctionsController : Controller
    {
        IDBHandler handler = new DBHandler();
        HttpCookie cookie;
        Functions function = new Functions();
        NotificationsFunctions notiFunctions = new NotificationsFunctions();

        public void getCookie(bool externalDownoladCheck)
        {
            try
            {
                //check if the user is loged in
                cookie = Request.Cookies["TaxAppUserID"];

                if (cookie != null)
                {
                    //show the nav tabs menue only for customers
                    if (cookie["ID"] != null || cookie["ID"] != "")
                    {
                        Model.Profile checkProfile = new Model.Profile();

                        checkProfile.ProfileID = int.Parse(cookie["ID"].ToString());
                        checkProfile.EmailAddress = "";
                        checkProfile.Username = "";

                        checkProfile = handler.getProfile(checkProfile);

                        if (checkProfile == null)
                        {
                            Response.Redirect(Url.Action("Welcome", "Landing"));
                        }

                        ViewBag.ProfileName = checkProfile.FirstName + " " + checkProfile.LastName;
                        ViewBag.NotificationList = notiFunctions.getNotifications(int.Parse(cookie["ID"]));
                    }
                    else
                    {
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    }
                }
                else if (externalDownoladCheck == true)
                {
                    cookie = Request.Cookies["TaxAppGuestUserID"];

                    if (cookie == null)
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    if (cookie["ID"] == null)
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                    if (cookie["ID"] == "")
                        Response.Redirect(Url.Action("Welcome", "Landing"));
                }
                else
                {
                    Response.Redirect(Url.Action("Welcome", "Landing"));
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Response.Redirect(Url.Action("Error", "Shared") + "?Err=Identity couldn't be verified");
            }
        }

        #region Upload File
        public ActionResult AttachFile(string ID, string type, string Title, string Details)
        {
            getCookie(false);
            ViewBag.Heading = Title;
            ViewBag.Details = Details;
            ViewBag.type = type;
            ViewBag.ID = ID;
             return View();
        }

        [HttpPost]
        public ActionResult AttachFile(string ID, string type, HttpPostedFileBase file)
        {
            try
            {
                getCookie(false);

                ID = Request.Form["ID"];
                type = Request.Form["type"];

                String FileExt = Path.GetExtension(file.FileName).ToUpper();

                Stream str = file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                Model.InvoiceAndReciptesFile newFile = new Model.InvoiceAndReciptesFile();
                newFile.fileByteArray = FileDet;
                newFile.fileName = file.FileName;

                newFile.ID = int.Parse(ID);

                bool success = false;

                if (type == "GE")
                {
                    success = handler.addGeneralExpenseFile(newFile);
                    if (success == true)
                        Response.Redirect(Url.Action("GeneralExpense", "Expense") + "?ID=" + ID);
                }
                else if (type == "JE")
                {
                    success = handler.addJobExpenseFile(newFile);
                    if (success == true)
                        Response.Redirect(Url.Action("JobExpense", "Expense") + "?ID="+ ID);
                }

                return RedirectToAction("Error", "Shared", new { Err = "An error occurred uploading the file" });

            }
            catch(Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                return RedirectToAction("Error", "Shared", new { Err = "An error occurred uploading the file" });
            }
        }
        #endregion
        
        #region DownLoad File
        public FileResult DownloadFile(string ID, string type)
        {
                getCookie(true);

            Model.InvoiceAndReciptesFile getFile = new Model.InvoiceAndReciptesFile();
            getFile.ID = int.Parse(ID);

                if (type == "GE")
                {
                getFile = handler.getGeneralExpenseFile(getFile);
                }
            else if (type == "JE")
                {
                getFile = handler.getJobExpenseFile(getFile);
                }
            else
            {
                function.logAnError("No Type Supplied - Doewnload FileFunctions controler");
                Response.Redirect(Url.Action("Error", "Shared"));
            }

                if(getFile == null)
                    Response.Redirect(Url.Action("Error", "Shared"));

            return File(getFile.fileByteArray, System.Net.Mime.MediaTypeNames.Application.Octet, getFile.fileName);
        }
        #endregion
    }
            
        
}