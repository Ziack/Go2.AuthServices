using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Web;

namespace Go2.AuthServices.LDAPIdp
{
    public class CertificateHelper
    {
        // The X509KeyStorageFlags.MachineKeySet flag is required when loading a
        // certificate from file on a shared hosting solution such as Azure.
        public static readonly X509Certificate2 SigningCertificate =
            new X509Certificate2(
                HttpContext.Current.Server.MapPath(
                "~\\App_Data\\Go2.AuthServices.LDAPIdp.pfx"), "",
                X509KeyStorageFlags.MachineKeySet);

        public static readonly KeyDescriptor SigningKey = 
            new KeyDescriptor(
            new SecurityKeyIdentifier(
                (new X509SecurityToken(SigningCertificate)).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>()));
    }
}