using System.Configuration;
using Go2.AuthServices.Saml2P;

namespace Go2.AuthServices.Configuration
{
    /// <summary>
    /// NamedId policy configuration element.
    /// </summary>
    public class NameIdPolicyElement : ConfigurationElement
    {
        /// <summary>
        /// Allow create.
        /// </summary>
        [ConfigurationProperty("allowCreate", IsRequired = false)]
        public bool? AllowCreate
        {
            get
            {
                return (bool?)base["allowCreate"];
            }
        }

        /// <summary>
        /// The NameId format.
        /// </summary>
        [ConfigurationProperty("format", IsRequired = false)]
        public NameIdFormat Format
        {
            get
            {
                return (NameIdFormat)base["format"];
            }
        }
    }
}