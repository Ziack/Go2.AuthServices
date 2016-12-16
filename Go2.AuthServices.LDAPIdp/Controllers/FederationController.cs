using Go2.AuthServices.LDAPIdp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go2.AuthServices.Metadata;

namespace Go2.AuthServices.LDAPIdp.Controllers
{
    public class FederationController : Controller
    {
        // GET: Federation
        public ActionResult Index()
        {
            return Content(
                MetadataModel.CreateFederationMetadata()
                .ToXmlString(CertificateHelper.SigningCertificate),
                "application/samlmetadata+xml");
        }

        public ActionResult BrowserFriendly()
        {
            return Content(
                MetadataModel.CreateFederationMetadata()
                .ToXmlString(CertificateHelper.SigningCertificate),
                "text/xml");
        }
    }
}