using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAutomation;

namespace Go2.AuthServices.IntegrationTests
{
    [TestClass]
    public class SignInTests : FluentTest
    {
        [TestInitialize]
        public void SignInTestsInitialize()
        {
            SeleniumWebDriver.Bootstrap(SeleniumWebDriver.Browser.Chrome);
            FluentAutomation.FluentSettings.Current.WaitOnAllActions = false;
        }        

        [TestMethod]
        public void SignInAndOut_IdpInitiated_HttpModule()
        {
            I.Open("http://localhost:52071/")
                .Enter("http://localhost:17009/SamplePath/AuthServices/Acs").In("#AssertionModel_AssertionConsumerServiceUrl")
                .Enter("http://localhost:17009/SamplePath/AuthServices").In("#AssertionModel_Audience")
                .Click("#submit")
                .Assert.Text("JohnDoe").In("tbody tr td:nth-child(2)");

            I.Open("http://localhost:52071/Logout")
                .Enter("JohnDoe").In("#NameId")
                .Enter("http://localhost:17009/SamplePath/AuthServices/Logout").In("#DestinationUrl")
                .Click("#submit")
                .Assert.Text("urn:oasis:names:tc:SAML:2.0:status:Success").In("#status");

            I.Open("http://localhost:17009/SamplePath")
                .Assert.Text("not signed in").In("#status");
        }        

        [TestMethod]
        public void SignInAndOut_SPInitiated_HttpModule_via_DiscoveryService()
        {
            I.Open("http://localhost:17009/SamplePath")
                .Click("a[href=\"/SamplePath/AuthServices/SignIn\"]")
                .Assert.Text("http://localhost:17009/SamplePath/AuthServices/SignIn").In("#return");

            I.Click("#submit")
                .Assert.Text("http://localhost:17009/SamplePath/AuthServices/Acs").In("#AssertionModel_AssertionConsumerServiceUrl");

            I.Assert.False(() => string.IsNullOrEmpty(I.Find("#AssertionModel_InResponseTo").Element.Value));

            I.Click("#submit")
                .Assert.Text("JohnDoe").In("tbody tr td:nth-child(2)");

            I.Click("#logout");

            I.Enter("http://localhost:17009/SamplePath/AuthServices/Logout").In("#DestinationUrl")
                .Click("#submit");

            I.Assert.Text("not signed in").In("#status");
            I.Assert.Url("http://localhost:17009/SamplePath/?Status=LoggedOut");
        }        
    }
}
