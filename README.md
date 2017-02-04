# Go2.AuthServices

Go2.AuthServices is a library that adds SAML2P support to ASP.NET and IIS
web sites, allowing the web site to act as a SAML2 Service Provider (SP).

## Using
The library can be used as an Http Module, loaded into the IIS pipeline. The module is compatible with ASP.NET web 
forms sites.

Note that this last usage scenario enables SAML identity providers to be integrated within
[IdentityServer3](https://github.com/IdentityServer/IdentityServer3) package.  Review [this document](doc/IdentityServer3Okta.md) to see how to configure AuthServices
with IdentityServer3 and Okta to add Okta as an identity provider to an IdentityServer3 project. There is also a SampleIdentityServer3 project in the AuthServices repository.

Once installed the `web.config` of the application must be updated with configuration.
See [configuration](doc/Configuration.md) for details.

## Troubleshooting

* Check the [issues archive](https://github.com/Ziack/Go2.AuthServices/issues).
* Check the [SAML2 specification](http://saml.xml.org/saml-specifications), starting with the core section.
* Log your actual SAML2 conversation with [SAML Chrome Panel](https://chrome.google.com/webstore/detail/saml-chrome-panel/paijfdbeoenhembfhkhllainmocckace) or [SAML Tracer for Firefox](https://addons.mozilla.org/sv-se/firefox/addon/saml-tracer/).
* Last but not least, download the AuthServices source and check out what's really happening.

## Saml2AuthenticationModule
The Saml2AuthenticationModule is modeled after the WSFederationAuthenticationModule
to provide Saml2 authentication to IIS web sites. In many cases it should just be
[configured](doc/Configuration.md) in and work without any code written in the application 
at all (even though [providing an own ClaimsAuthenticationManager](doc/ClaimsAuthenticationManager.md)
for claims translation is highly recommended).

## Stub Idp
The solution also contains a stub (i.e. dummy) identity provider that can be used for testing.
Download the solution, or use the instance that's provided for free at http://stubidp.kentor.se.

## Protocol Classes
The protocol handling classes are available as a public API as well, making it possible to 
reuse some of the internals for writing your own service provider or identity provider.
