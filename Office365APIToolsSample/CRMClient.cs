using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    public class CrmClient : Office365Client
    {
        private const string ResourceName = "Microsoft.CRM";

        public CrmClient(Authenticator<FixedSessionCache> authenticator) : base(authenticator)
        {
            
        }
        private AuthenticationInfo _authenticationInfo = null;

        public async override Task<AuthenticationInfo> GetAuthenticationInfo()
        {
            if (_authenticationInfo == null)
            {
                _authenticationInfo = await authenticator.AuthenticateAsync(ResourceName, ServiceIdentifierKind.Resource);
            }

            return _authenticationInfo;
        }
    }
}