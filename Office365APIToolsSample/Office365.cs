using Microsoft.Office365.OAuth;

namespace Office365APIToolsSample
{
    static class Office365
    {
        public static Authenticator<FixedSessionCache> autenticator = new Authenticator<FixedSessionCache>();

        public static OneDriveClient OneDriveClient
        {
            get
            {
                return new OneDriveClient(autenticator);
            }
        }

        public static CrmClient CrmClient
        {
            get
            {
                return new CrmClient(autenticator);
            }
        }
    }
}