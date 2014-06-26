using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using Microsoft.Office365.OAuth;
using Microsoft.Office365.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office365APIToolsSample;

namespace Office365APIToolsSample
{
    static class MyFilesApiSample
    {
        const string MyFilesCapability = "MyFiles";

        public static async Task<IEnumerable<IFileSystemItem>> GetMyFiles()
        {
            var client = await EnsureClientCreated();
            client.Context.IgnoreMissingProperties = true; //Must be set to avoid exception

            // Obtain files in folder "Shared with Everyone"
            var filesResults = await client.Files["Shared with Everyone"].ToFolder().Children.ExecuteAsync();
            var files = filesResults.CurrentPage.OrderBy(e => e.Name);

            return files;
        }

        public static async Task<IEnumerable<File>> GetFiles()
        {
            var client = Office365.OneDriveClient;

            string filesUrl = String.Format(CultureInfo.InvariantCulture,
                "/web/lists/GetByTitle('Documents')/Files?$select=Name");


            var files = await client.Get<IEnumerable<File>>(filesUrl);

            return files;
        }

        private static async Task<SharePointClient> EnsureClientCreated()
        {
            var authenticator = new Authenticator<FixedSessionCache>();
            var authInfo = await authenticator.AuthenticateAsync(MyFilesCapability, ServiceIdentifierKind.Capability);
            // Create the MyFiles client proxy:
            return new SharePointClient(authInfo.ServiceUri, authInfo.GetAccessToken);
        }
        public static void SignOut(Uri postLogoutRedirect)
        {
            new Authenticator().Logout(postLogoutRedirect);
        }
    }
}
