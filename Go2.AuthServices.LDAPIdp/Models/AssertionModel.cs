﻿using Go2.AuthServices.Saml2P;
using Go2.AuthServices.WebSso;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;

namespace Go2.AuthServices.LDAPIdp.Models
{
    public class AssertionModel
    {
        [Required]
        [Display(Name = "Assertion Consumer Service Url")]
        public string AssertionConsumerServiceUrl { get; set; }

        [Display(Name = "Relay State")]
        [StringLength(80)]
        public string RelayState { get; set; }

        [Display(Name = "Subject NameId")]
        [Required]
        public string NameId { get; set; }

        [Display(Name = "Audience")]
        public string Audience { get; set; }

        public ICollection<AttributeStatementModel> AttributeStatements { get; set; }

        /// <summary>
        /// Creates a new Assertion model
        /// </summary>
        /// <returns>An <see cref="AssertionModel"/></returns>
        public static AssertionModel Create(String nameId)
        {
            return new AssertionModel
            {
                AssertionConsumerServiceUrl = ConfigurationManager.AppSettings["defaultAcsUrl"],
                NameId = nameId,
                SessionIndex = Convert.ToString(Guid.NewGuid())
            };
        }

        [Display(Name = "In Response To ID")]
        public string InResponseTo { get; set; }

        private static IEnumerable<string> YieldIfNotNullOrEmpty(string src)
        {
            if(!string.IsNullOrEmpty(src))
            {
                yield return src;
            }
        }

        public Saml2Response ToSaml2Response()
        {
            var nameIdClaim = new Claim(ClaimTypes.NameIdentifier, NameId);
            nameIdClaim.Properties[ClaimProperties.SamlNameIdentifierFormat] = 
                NameIdFormat.Unspecified.GetUri().AbsoluteUri;
            var claims =
                new Claim[] { nameIdClaim }
                .Concat(YieldIfNotNullOrEmpty(SessionIndex).Select(
                    s => new Claim(AuthServicesClaimTypes.SessionIndex, SessionIndex)))
                .Concat((AttributeStatements ?? Enumerable.Empty<AttributeStatementModel>())
                    .Select(att => new Claim(att.Type, att.Value)));
            var identity = new ClaimsIdentity(claims);

            Saml2Id saml2Id = null;
            if (!string.IsNullOrEmpty(InResponseTo))
            {
                saml2Id = new Saml2Id(InResponseTo);
            }
            
            var audienceUrl = string.IsNullOrEmpty(Audience)
                ? null
                : new Uri(Audience);

            return new Saml2Response(
                new EntityId(UrlResolver.MetadataUrl.ToString()),
                CertificateHelper.SigningCertificate, new Uri(AssertionConsumerServiceUrl),
                saml2Id, RelayState, audienceUrl, identity);
        }

        [Display(Name = "Incoming AuthnRequest")]
        public string AuthnRequestXml { get; set; }

        public Saml2BindingType ResponseBinding { get; set; } = Saml2BindingType.HttpPost;

        [Display(Name = "Session Index")]
        public String SessionIndex { get; set; }
    }
}