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

        public void getCookie()
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
                            Response.Redirect("/Landing/Welcome");
                        }

                        ViewBag.ProfileName = checkProfile.FirstName + " " + checkProfile.LastName;
                    }
                    else
                    {
                        Response.Redirect("/Landing/Welcome");
                    }
                }
                else
                {
                    Response.Redirect("/Landing/Welcome");
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in welcome method of LandingControles");
                Redirect("/Shared/Error");
            }
        }

        #region Upload File
        public ActionResult AttachFile(string ID, string type, string Title, string Details)
        {
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
                getCookie();

                ID = Request.Form["ID"];
                type = Request.Form["type"];

                String FileExt = Path.GetExtension(file.FileName).ToUpper();

                Stream str = file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                FileUpload newFile = new FileUpload();
                newFile.fileByteArray = FileDet;
                newFile.fileName = file.FileName;

                newFile.ID = int.Parse(ID);

                bool success = false;

                if (type == "GE")
                {
                    success = handler.addGeneralExpenseFile(newFile);
                    if (success == true)
                        Response.Redirect("../Expense/GeneralExpense?ID="+ID);
                }
                else if (type == "JE")
                {
                    success = handler.addJobExpenseFile(newFile);
                    if (success == true)
                        Response.Redirect("../Expense/JobExpense?ID="+ID);
                }

                Response.Redirect("../Shared/Error?Err=An error occurred uploading file");

            }
            catch(Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error loding Vat Center");
                return Redirect("../Shared/Error?Err=An error occurred uploading file");
            }

            return Redirect("../Shared/Error?Err=An error occurred uploading the file");
        }
        #endregion
        
        #region DownLoad File
        public FileResult DownloadFile(string ID, string type)
        {
                getCookie();

                FileUpload getFile = new FileUpload();
            getFile.ID = int.Parse(ID);

                if (type == "GE")
                {
                getFile = handler.getGeneralExpenseFile(getFile);
                }
            else if (type == "JE")
                {
                getFile = handler.getJobExpenseFile(getFile);
                }

            return File(getFile.fileByteArray, System.Net.Mime.MediaTypeNames.Application.Octet, getFile.fileName);
        }
        #endregion
    }
            
        
}