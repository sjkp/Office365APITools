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


        public CrmClient(Authenticator<FixedSessionCache> authenticator)
            : base(authenticator)
        {

        }
        private AuthenticationInfo _authenticationInfo = null;

        public async override Task<AuthenticationInfo> GetAuthenticationInfo()
        {
            if (_authenticationInfo == null)
            {
                _authenticationInfo = await authenticator.AuthenticateAsync(ResourceId, ResourceType);
            }

            return _authenticationInfo;
        }

        public override string ResourceId
        {
            get { return "Microsoft.CRM"; }
        }

        public override ServiceIdentifierKind ResourceType
        {
            get { return ServiceIdentifierKind.Resource; }
        }
    }
}