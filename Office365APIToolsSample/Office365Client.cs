using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Office365.OAuth;
using Newtonsoft.Json.Linq;

namespace Office365APIToolsSample
{
    public abstract class Office365Client
    {
        protected Authenticator<FixedSessionCache> authenticator;

        protected Office365Client(Authenticator<FixedSessionCache> authenticator)
        {
            this.authenticator = authenticator;
        }

        public abstract Task<AuthenticationInfo> GetAuthenticationInfo();

        /// <summary>
        /// Gets the data from the endpoint relative REST url. 
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string requestUrl) 
        {
            var authInfo = await GetAuthenticationInfo();
            var url = authInfo.ServiceUri + requestUrl;
            if (requestUrl.StartsWith("http"))
            {
                url = requestUrl;
            }
            var res = await Execute(url, HttpMethod.Get);

            var jobject = JObject.Parse(res);

            if (typeof (T).GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return jobject["d"]["results"].ToObject<T>();    
            }
            return jobject["d"].ToObject<T>();

        }

        private async Task<string> Execute(string requestUrl, HttpMethod httpMethod, HttpContent content = null)
        {
            // Prepare the HTTP request:
            using (HttpClient client = new HttpClient())
            {
                Func<HttpRequestMessage> requestCreator = () =>
                {
                    HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUrl);
                    request.Headers.Add("Accept", "application/json;odata=verbose"); //application/json;odata=minimalmetadata

                    request.Content = content;
                    return request;
                };

                // Send the request using a helper method, which will add an authorization header to the request,
                // and automatically retry with a new token if the existing one has expired.
                var authenticationInfo = await GetAuthenticationInfo();
                using (HttpResponseMessage response = await SendRequestAsync(authenticationInfo, client, requestCreator))
                {
                    // Read the response and deserialize the data:
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new InvalidOperationException(responseString);
                    }
                    return responseString;
                }
            }
        }

        /// <summary>
        /// Send an HTTP request, with authorization. If the request fails due to an unauthorized exception,
        ///     this method will try to renew the access token in serviceInfo and try again.
        /// </summary>
        public async Task<HttpResponseMessage> SendRequestAsync(
            AuthenticationInfo authInfo, HttpClient client, Func<HttpRequestMessage> requestCreator)
        {
            using (HttpRequestMessage request = requestCreator.Invoke())
            {
                string accessToken = await authInfo.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Add("X-ClientService-ClientTag", new[] { "Office 365 API Tools", "1.1" });
                HttpResponseMessage response = await client.SendAsync(request);

                // Check if the server responded with "Unauthorized". If so, it might be a real authorization issue, or 
                //     it might be due to an expired access token. To be sure, renew the token and try one more time:
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                        
                    //Refresh accessToken 
                    accessToken = await (await GetAuthenticationInfo()).GetAccessToken();

                    // Create and send a new request:
                    using (HttpRequestMessage retryRequest = requestCreator.Invoke())
                    {
                        retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        retryRequest.Headers.Add("X-ClientService-ClientTag", new[] { "Office 365 API Tools", "1.1" });
                        response = await client.SendAsync(retryRequest);
                    }
                }

                // Return either the original response, or the response from the second attempt:
                return response;
            }
        }
    }
}