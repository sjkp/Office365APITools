using System.Threading.Tasks;
using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    public class OneDriveClient : Office365Client
    {        
        public OneDriveClient(Authenticator<FixedSessionCache> authenticator) : base(authenticator)
        {
            
        }

        private AuthenticationInfo _authenticationInfo = null;

        public override async Task<AuthenticationInfo> GetAuthenticationInfo()
        {
            if (_authenticationInfo == null)
            {
                _authenticationInfo = await authenticator.AuthenticateAsync(ResourceId, ResourceType);
            }
                
            return _authenticationInfo;
        }

        public override string ResourceId
        {
            get { return "MyFiles"; }
        }

        public override ServiceIdentifierKind ResourceType
        {
            get { return ServiceIdentifierKind.Capability; }
        }
    }
}