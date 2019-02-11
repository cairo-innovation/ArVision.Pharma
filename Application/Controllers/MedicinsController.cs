using ArVision.Core.Logging;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pharma.Controllers
{
    public class MedicinsController : BaseController
    {
        //private Entities db = new Entities();

        // GET: Medicins
        public ActionResult Index()
        {
            //aboziad//should be replaced by a service call 
            CLASS_NAME = ControllerContext.CurrentClassName();
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            if (pharmaServiceClient == null)
            {
                SERVICE_URL = ConfigurationManager.AppSettings["API_URL"];
                string port = ConfigurationManager.AppSettings["TCP_PORT"];
                int TCP_PORT = int.Parse(port);
                pharmaServiceClient = new PharmaServiceFactory().GetPharmaServiceProxy(SERVICE_URL, TCP_PORT);

            }
            var medicins = pharmaServiceClient.GetList("Medicine").ToList();//aboziad//should be replaced by a service call 
            return View(medicins);
        }

        // GET: Medicins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicin medicin = null;//aboziad//should be replaced by a service call  db.Medicins.Find(id);
            if (medicin == null)
            {
                return HttpNotFound();
            }
            return View(medicin);
        }

        // GET: Medicins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MidicinName")] Medicin medicin)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call 
                //db.Medicins.Add(medicin);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicin);
        }

        // GET: Medicins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicin medicin = null;//aboziad//should be replaced by a service call  //db.Medicins.Find(id);
            if (medicin == null)
            {
                return HttpNotFound();
            }
            return View(medicin);
        }

        // POST: Medicins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MidicinName")] Medicin medicin)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call 
                //db.Entry(medicin).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicin);
        }

        // GET: Medicins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicin medicin = null;//aboziad//should be replaced by a service call // db.Medicins.Find(id);
            if (medicin == null)
            {
                return HttpNotFound();
            }
            return View(medicin);
        }

        // POST: Medicins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicin medicin = null;//aboziad//should be replaced by a service call  db.Medicins.Find(id);
            //db.Medicins.Remove(medicin);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //aboziad//should be replaced by a service call 
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
