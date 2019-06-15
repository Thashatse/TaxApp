using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace TaxApp.Controllers
{
    public class LandingController : Controller
    {
        IDBHandler handler = new DBHandler();

        // GET: Landing
        public ActionResult Welcome()
        {
            //Model.Business business = handler.GetBussiness();
            //ViewBag.Message = business.BusinessID.ToString() + " " + business.VATRate.ToString() + " " + business.SMSSid + " " + business.SMSToken;
            return View();
        }

        // GET: Landing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Landing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Landing/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Landing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Landing/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Landing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Landing/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
