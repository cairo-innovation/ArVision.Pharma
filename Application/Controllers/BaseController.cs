using ArVision.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharma.Controllers
{
    public class BaseController : Controller
    {
        protected string CLASS_NAME;// = nameof(JuicesController);http://localhost:5671/
        protected static string SERVICE_URL = "127.0.0.1";
        //protected const string SERVICE_URL = "localhost";

        //private Entities db = new Entities();

        protected PharmaServiceProxy pharmaServiceClient;
    }
}