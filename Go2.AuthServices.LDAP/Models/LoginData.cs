using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Go2.AuthServices.LDAP.Models
{
    [DataContract]
    public class LoginData
    {
        [Required]        
        [DataMember(Name = "username")]
        [EmailAddress]
        public String Username;

        [Required]
        [DataMember(Name = "password")]
        public String Password;        
    }
}