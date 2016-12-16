using Go2.AuthServices.LDAPIdp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Go2.AuthServices.Metadata;

namespace Go2.AuthServices.LDAPIdp.Controllers
{
    public class MetadataController : Controller
    {
        // GET: Metadata
        public ActionResult Index()
        {
            return Content(
                MetadataModel.CreateIdpMetadata()
                .ToXmlString(CertificateHelper.SigningCertificate),
                "application/samlmetadata+xml");
        }

        public ActionResult BrowserFriendly()
        {
            return Content(
                MetadataModel.CreateIdpMetadata()
                .ToXmlString(CertificateHelper.SigningCertificate),
                "text/xml");
        }
    }
}