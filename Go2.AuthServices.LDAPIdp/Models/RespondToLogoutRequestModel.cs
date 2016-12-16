using Go2.AuthServices.Saml2P;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace Go2.AuthServices.LDAPIdp.Models
{
    public class RespondToLogoutRequestModel
    {
        [DisplayName("Received LogoutRequest")]
        public string LogoutRequestXml { get; set; }

        [DisplayName("InResponseTo")]
        public string InResponseTo { get; set; }

        [Required]
        [DisplayName("SP Single Logout Url")]
        public Uri DestinationUrl { get; set; }

        [DisplayName("Relay State")]
        public string RelayState { get; set; }

        public Saml2LogoutResponse ToLogoutResponse()
        {
            return new Saml2LogoutResponse(Saml2StatusCode.Success)
            {
                DestinationUrl = DestinationUrl,
                SigningCertificate = CertificateHelper.SigningCertificate,
                InResponseTo = new Saml2Id(InResponseTo),
                Issuer = new EntityId(UrlResolver.MetadataUrl.ToString()),
                RelayState = RelayState
            };
        }
    }
}