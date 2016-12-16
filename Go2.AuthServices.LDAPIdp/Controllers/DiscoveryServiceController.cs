using Go2.AuthServices.LDAPIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go2.AuthServices.LDAPIdp.Controllers
{
    public class DiscoveryServiceController : Controller
    {
        public ActionResult Index(DiscoveryServiceModel model)
        {
            String delimiter = model.@return.Contains("?") ? "&" : "?";
            var redirectToURL = $"{ model.@return }{ delimiter }{ model.returnIDParam }={ model.SelectedIdp }";

            return Redirect(redirectToURL);

        }
    }
}