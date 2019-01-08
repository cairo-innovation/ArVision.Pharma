using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArVision.Pharma.Shared.DataModels;

namespace Pharma.Controllers
{
    public class visitsController : Controller
    {
        //private Entities db = new Entities();

        // GET: visits
        public ActionResult Index()
        {
            //var patientVisits = db.PatientVisits.Include(p => p.Doctor).Include(p => p.Juice).Include(p => p.Medicin).Include(p => p.Patient).Include(p => p.Rx).Include(p => p.User);

            PatientVisit patientVisits = null;
            return View(patientVisits);
        }

        // GET: visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientVisit = null;//aboziad//should be replaced by a service call// b.PatientVisits.Include(p => p.Doctor).Include(p => p.Juice).Include(p => p.Medicin).Include(p => p.Patient).Include(p => p.Rx).Include(p => p.User).FirstOrDefault(f=>f.Id==id);
            if (patientVisit == null)
            {
                return HttpNotFound();
            }
            //aboziad//should be replaced by a service call
            // ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "DoctorName", patientVisit.DoctorId);
            //ViewBag.JuiceId = new SelectList(db.Juices, "Id", "JuiceName", patientVisit.JuiceId);
            //ViewBag.MedicinId = new SelectList(db.Medicins, "Id", "MidicinName", patientVisit.MedicinId);
            return View(patientVisit);
        }

        // GET: visits/Create
        public ActionResult Create()
        {
            //aboziad//should be replaced by a service call
            //ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "DoctorName");
            //ViewBag.JuiceId = new SelectList(db.Juices, "Id", "JuiceName");
            //ViewBag.MedicinId = new SelectList(db.Medicins, "Id", "MidicinName");
            //ViewBag.PatientId = new SelectList(db.Patients, "Id", "PatientName");
            //ViewBag.RxId = new SelectList(db.Rxes, "Id", "Id");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,PatientId,DoctorId,MedicinId,JuiceId,RxId,DayWeek,VisitDate")] PatientVisit patientVisit)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call
                //db.PatientVisits.Add(patientVisit);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            //aboziad//should be replaced by a service call
            //ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "DoctorName", patientVisit.DoctorId);
            //ViewBag.JuiceId = new SelectList(db.Juices, "Id", "JuiceName", patientVisit.JuiceId);
            //ViewBag.MedicinId = new SelectList(db.Medicins, "Id", "MidicinName", patientVisit.MedicinId);
            //ViewBag.PatientId = new SelectList(db.Patients, "Id", "PatientName", patientVisit.PatientId);
            //ViewBag.RxId = new SelectList(db.Rxes, "Id", "Id", patientVisit.RxId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", patientVisit.UserId);
            return View(patientVisit);
        }

        // GET: visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientVisit = null;//aboziad//should be replaced by a service call// db.PatientVisits.Find(id);
            if (patientVisit == null)
            {
                return HttpNotFound();
            }
            //aboziad//should be replaced by a service call
            //ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "DoctorName", patientVisit.DoctorId);
            //ViewBag.JuiceId = new SelectList(db.Juices, "Id", "JuiceName", patientVisit.JuiceId);
            //ViewBag.MedicinId = new SelectList(db.Medicins, "Id", "MidicinName", patientVisit.MedicinId);
            //ViewBag.PatientId = new SelectList(db.Patients, "Id", "PatientName", patientVisit.PatientId);
            //ViewBag.RxId = new SelectList(db.Rxes, "Id", "Id", patientVisit.RxId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", patientVisit.UserId);
            return View(patientVisit);
        }

        // POST: visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,PatientId,DoctorId,MedicinId,JuiceId,RxId,DayWeek,VisitDate")] PatientVisit patientVisit)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call
                //db.Entry(patientVisit).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            //aboziad//should be replaced by a service call
            //ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "DoctorName", patientVisit.DoctorId);
            //ViewBag.JuiceId = new SelectList(db.Juices, "Id", "JuiceName", patientVisit.JuiceId);
            //ViewBag.MedicinId = new SelectList(db.Medicins, "Id", "MidicinName", patientVisit.MedicinId);
            //ViewBag.PatientId = new SelectList(db.Patients, "Id", "PatientName", patientVisit.PatientId);
            //ViewBag.RxId = new SelectList(db.Rxes, "Id", "Id", patientVisit.RxId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", patientVisit.UserId);
            return View(patientVisit);
        }

        // GET: visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientVisit patientVisit = null;//aboziad//should be replaced by a service call// db.PatientVisits.Find(id);
            if (patientVisit == null)
            {
                return HttpNotFound();
            }
            return View(patientVisit);
        }

        // POST: visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientVisit patientVisit = null;////aboziad//should be replaced by a service call// db.PatientVisits.Find(id);
            //db.PatientVisits.Remove(patientVisit);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
