using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Go2.AuthServices.HttpModule
{
    internal class Saml2AuthenticationContextWrapper : ISaml2AuthenticationContextWrapper
    {
        public Saml2AuthenticationContextWrapper()
        {
            HttpContextWrapper = new HttpContextWrapper(HttpContext.Current);
        }

        public HttpContextWrapper HttpContextWrapper { get; private set; }
    }
}
