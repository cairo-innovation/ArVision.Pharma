using ArVision.Base.Shared;
using ArVision.Pharma.DataAccess;
using ArVision.Service.Pharma.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ArVision.Pharma.Api.Controllers
{
    public class BaseController : ArVisionBaseApiController
    {
        protected IPharmaService baseService;
        public BaseController(IPharmaService baseService, IServiceStatusProvider serviceStatusProvider) : base(serviceStatusProvider)
        {
            this.baseService = baseService ?? throw new System.ArgumentNullException(nameof(baseService));
        }
        //protected IPharmaRepository pharmaRepositoy = new PharmaRepository();
    }
}
