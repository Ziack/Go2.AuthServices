using System.Web;

namespace Go2.AuthServices.HttpModule
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISaml2AuthenticationContextWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        HttpContextWrapper HttpContextWrapper { get; }
    }
}
