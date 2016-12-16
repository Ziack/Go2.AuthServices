using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Go2.AuthServices.LDAP.Helpers
{
    public static class StringExtension
    {
        public static String ToDistinguishedName(this String email)
        {
            MailAddress addr = new MailAddress(email);
            String uid = addr.User;
            String[] domainParts = addr.Host.Split(new[] { '.' });

            var distinguishedNameParts = new List<String>();
            distinguishedNameParts.Add($"uid={ uid }");

            Array.ForEach(domainParts, t => distinguishedNameParts.Add($"dc={ t }"));

            return String.Join(",", distinguishedNameParts);
        }
    }
}