using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArVision.Pharma.Shared.DataModels;

namespace Pharma.Controllers
{
    public class RxesController : Controller
    {
        //private Entities db = new Entities();

        // GET: Rxes
        public async Task<ActionResult> Index()
        {
            List<Rx> rxes = new List<Rx>();
            //aboziad//should be replaced by a service call
            return View(rxes /*await rxes*/);
        }

        // GET: Rxes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rx rx = null; //aboziad//should be replaced by a service call// await db.Rxes.FindAsync(id);
            if (rx == null)
            {
                return HttpNotFound();
            }
            return View(rx);
        }

        // GET: Rxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IMG,CreatedDate")] Rx rx)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call
                //db.Rxes.Add(rx);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rx);
        }

        // GET: Rxes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rx rx = null; //aboziad//should be replaced by a service call //await db.Rxes.FindAsync(id);
            if (rx == null)
            {
                return HttpNotFound();
            }
            return View(rx);
        }

        // POST: Rxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IMG,CreatedDate")] Rx rx)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call
                //db.Entry(rx).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rx);
        }

        // GET: Rxes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rx rx = null;////aboziad//should be replaced by a service call// await db.Rxes.FindAsync(id);
            if (rx == null)
            {
                return HttpNotFound();
            }
            return View(rx);
        }

        // POST: Rxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rx rx = null;//aboziad//should be replaced by a service call            //await db.Rxes.FindAsync(id);
            //db.Rxes.Remove(rx);
            //await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ////aboziad//should be replaced by a service call//db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
