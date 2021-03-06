﻿using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Xml.Linq;

namespace Go2.AuthServices.IntegrationTests
{
    [TestClass]
    public class MetadataTests
    {
        [TestMethod]
        public void Metadata_GetMetadata_Saml2AuthenticationModule()
        {
            var url = "http://localhost:17009/SamplePath/AuthServices/";

            TestMetadata(url);
        }

        private static void TestMetadata(string url)
        {
            var client = new WebClient();
            var metadata = client.DownloadString(url);
            var mimeType = client.ResponseHeaders["Content-Type"];

            mimeType.Should().Contain("application/samlmetadata+xml");

            XDocument.Parse(metadata).Root.Name.Should()
                .Be(XNamespace.Get("urn:oasis:names:tc:SAML:2.0:metadata") + "EntityDescriptor");
        }        
    }
}
