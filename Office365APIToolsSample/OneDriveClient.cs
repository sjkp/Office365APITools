using System.Threading.Tasks;
using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    public class OneDriveClient : Office365Client
    {        
        public OneDriveClient(Authenticator<FixedSessionCache> authenticator) : base(authenticator)
        {
            
        }

        const string MyFilesCapability = "MyFiles";

        private AuthenticationInfo _authenticationInfo = null;

        public override async Task<AuthenticationInfo> GetAuthenticationInfo()
        {
            if (_authenticationInfo == null)
            {
                _authenticationInfo = await authenticator.AuthenticateAsync(MyFilesCapability, ServiceIdentifierKind.Capability);
            }
                
            return _authenticationInfo;
        }
    }
}