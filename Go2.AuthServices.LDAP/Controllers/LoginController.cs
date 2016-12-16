using Go2.AuthServices.LDAP.Helpers;
using Go2.AuthServices.LDAP.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Go2.AuthServices.LDAP.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login(LoginData loginData)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var loginIsSuccess = LDAPClient.Instance.ValidateUserByBind(username: loginData.Username.ToDistinguishedName(), password: loginData.Password);

            if (loginIsSuccess)
                return new HttpResponseMessage(HttpStatusCode.OK);
            
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}