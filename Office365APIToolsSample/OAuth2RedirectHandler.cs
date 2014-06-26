using System.Web.SessionState;
using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    public class OAuth2RedirectHandler : OAuth2RedirectHandler<FixedSessionCache>, IRequiresSessionState
    {
    }
}