using Go2.AuthServices.LDAPIdp.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Go2.AuthServices.Saml2P;
using Go2.AuthServices.WebSso;
using Go2.AuthServices.HttpModule;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Go2.AuthServices.LDAPIdp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var requestData = Request.ToHttpRequestData(true);

            if (requestData.QueryString["SAMLRequest"].Any())
                return View();

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<ActionResult> Index(HomePageModel model)
        {
            if (ModelState.IsValid)
            {

                var LDAPEndpoint = ConfigurationManager.AppSettings["LDAP.Endpoint"];

                if (String.IsNullOrEmpty(LDAPEndpoint))
                    throw new ConfigurationErrorsException("ConfigurationManager.AppSettings[\"LDAP.Endpoint\"] should not be null.");

                var client = new HttpClient();
                var result = await client.PostAsJsonAsync(LDAPEndpoint, new { Username = model.Username, Password = model.Password });

                if (!result.IsSuccessStatusCode)
                {
                    if(result.StatusCode == HttpStatusCode.Unauthorized)
                        ModelState.AddModelError("LDAP.Endpoint", "Username or Password not valid.");
                    else
                        ModelState.AddModelError("LDAP.Endpoint", "An error has ocurred. Please contact administrator.");

                    return View(model);
                }                    

                var assertionModel = AssertionModel.Create(nameId: model.Username);

                var requestData = Request.ToHttpRequestData(true);

                if (requestData.QueryString["SAMLRequest"].Any())
                {
                    var extractedMessage = Saml2Binding.Get(Saml2BindingType.HttpRedirect)
                    .Unbind(requestData, null);

                    var request = new Saml2AuthenticationRequest(
                        extractedMessage.Data,
                        extractedMessage.RelayState);

                    assertionModel.InResponseTo = request.Id.Value;
                    assertionModel.AssertionConsumerServiceUrl = request.AssertionConsumerServiceUrl.ToString();
                    assertionModel.RelayState = extractedMessage.RelayState;
                    assertionModel.Audience = request.Issuer.Id;
                    assertionModel.AuthnRequestXml = extractedMessage.Data.PrettyPrint();

                    var response = assertionModel.ToSaml2Response();

                    return Saml2Binding.Get(assertionModel.ResponseBinding)
                        .Bind(response).ToActionResult();
                }
                    
            }

            return View(model);
        }
    }
}