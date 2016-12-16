using Go2.AuthServices.HttpModule;
using Go2.AuthServices.Saml2P;
using Go2.AuthServices.LDAPIdp.Models;
using Go2.AuthServices.WebSso;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Go2.AuthServices.LDAPIdp.Controllers
{
    public class LogoutController : Controller
    {
        public ActionResult Index()
        {
            var requestData = Request.ToHttpRequestData(true);

            if (requestData.QueryString["SAMLRequest"].Any())
                return View();

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        
        [HttpPost]
        public ActionResult RespondToLogoutRequest()
        {
            var requestData = Request.ToHttpRequestData(true);
            var binding = Saml2Binding.Get(requestData);
            var unbindResult = binding.Unbind(requestData, null);

            var logoutRequest = Saml2LogoutRequest.FromXml(unbindResult.Data);

            var model = new RespondToLogoutRequestModel()
            {
                LogoutRequestXml = unbindResult.Data.PrettyPrint(),
                InResponseTo = logoutRequest.Id.Value,
                DestinationUrl = new Uri(new Uri(logoutRequest.Issuer.Id + "/"), "Logout"),
                RelayState = Request.QueryString["RelayState"]
            };

            return Saml2Binding.Get(Saml2BindingType.HttpRedirect)
                .Bind(model.ToLogoutResponse())
                .ToActionResult();
        }   
    }
}