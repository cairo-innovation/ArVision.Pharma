
using System.Linq;
using System.Net;

using System.Web.Mvc;
using ArVision.Core.Logging;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Client;

namespace Pharma.Controllers
{
    public class JuicesController : BaseController
    {
        //private const string CLASS_NAME = nameof(JuicesController);
        //private const string SERVICE_URL = "127.0.0.1";

        ////private Entities db = new Entities();

        //private PharmaServiceProxy pharmaServiceClient;


        // GET: Juices
        public ActionResult Index()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            if (pharmaServiceClient == null)
            {
                pharmaServiceClient = new PharmaServiceFactory().GetPharmaServiceProxy(SERVICE_URL);

            }

            //return View(pharmaServiceClient.GetJuiceList().ToList());
            return View(pharmaServiceClient.GetList("Juice").ToList());
        }

        // GET: Juices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juice juice = null;////aboziad//should be replaced by a service call  db.Juices.Find(id);
            if (juice == null)
            {
                return HttpNotFound();
            }
            return View(juice);
        }

        // GET: Juices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Juices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JuiceName")] Juice juice)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call 

                //db.Juices.Add(juice);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(juice);
        }

        // GET: Juices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juice juice = null; ////aboziad//should be replaced by a service call  db.Juices.Find(id);
            if (juice == null)
            {
                return HttpNotFound();
            }
            return View(juice);
        }

        // POST: Juices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JuiceName")] Juice juice)
        {
            if (ModelState.IsValid)
            {
                //aboziad//should be replaced by a service call 
                //db.Entry(juice).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(juice);
        }

        // GET: Juices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juice juice = null;////aboziad//should be replaced by a service call  db.Juices.Find(id);
            if (juice == null)
            {
                return HttpNotFound();
            }
            return View(juice);
        }

        // POST: Juices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Juice juice = null; //aboziad//should be replaced by a service call to find a Juice
            //db.Juices.Remove(juice);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //aboziad // dispose refe db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
