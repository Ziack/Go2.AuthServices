using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Configuration;

namespace Go2.AuthServices.LDAP
{
    /// <summary>
    /// A sample LDAP client. For simplicity reasons, this clients only uses synchronous requests.
    /// </summary>
    public class LDAPClient
    {
        private static LDAPClient instance;

        private LdapConnection connection;

        public static LDAPClient Instance
        {
            get
            {
                if (instance == null)
                {
                    var username = ConfigurationManager.AppSettings["LDAPClient.Username"];
                    var password = ConfigurationManager.AppSettings["LDAPClient.Password"];
                    var host = ConfigurationManager.AppSettings["LDAPClient.Host"];                    

                    if (String.IsNullOrEmpty(username))
                        throw new ConfigurationErrorsException("ConfigurationManager.AppSettings[\"LDAPClient.Username\"] should not be null.");

                    if (String.IsNullOrEmpty(password))
                        throw new ConfigurationErrorsException("ConfigurationManager.AppSettings[\"LDAPClient.Password\"] should not be null.");

                    if (String.IsNullOrEmpty(host))
                        throw new ConfigurationErrorsException("ConfigurationManager.AppSettings[\"LDAPClient.Host\"] should not be null.");

                    Int32 port;

                    instance = new LDAPClient(
                        username: username,
                        password: password,
                        host: host,
                        port: Int32.TryParse(ConfigurationManager.AppSettings["LDAPClient.Port"], out port) ? port : 389
                        );
                }
                return instance;
            }
        }

        private LDAPClient(String username, String password, String host, Int32 port)
        {
            var credentials = new NetworkCredential(username, password);
            
            var serverId = new LdapDirectoryIdentifier(host, port, true, false);

            connection = new LdapConnection(serverId, credentials);
            connection.AuthType = AuthType.Basic;
            connection.SessionOptions.ProtocolVersion = 3;

            connection.Bind();
        }              

        /// <summary>
        /// Another way of validating a user is by performing a bind. In this case the server
        /// queries its own database to validate the credentials. It is defined by the server
        /// how a user is mapped to its directory.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>true if the credentials are valid, false otherwise</returns>
        public bool ValidateUserByBind(String username, String password)
        {
            bool result = true;
            var credentials = new NetworkCredential(username, password);
            var serverId = new LdapDirectoryIdentifier(connection.SessionOptions.HostName);

            var conn = new LdapConnection(serverId, credentials);
            conn.AuthType = AuthType.Basic;
            conn.SessionOptions.ProtocolVersion = 3;

            try
            {
                conn.Bind();
            }
            catch (Exception)
            {
                result = false;
            }

            conn.Dispose();

            return result;
        }        
    }
}