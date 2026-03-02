using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Auth
{
    public class GetMe : CodeActivity
    {
        #region Constants


        #endregion

        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<AuthUserResponse> UserDetails { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            AuthUserResponse userDate = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                UserDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            userDate = GetUser(out errorMessage);

            if (userDate == null)
            {
                Trace.TraceError($"Getting user details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                UserDetails.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting user details succeeded.");
                UserDetails.Set(context, userDate);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Authentication

        private AuthUserResponse GetUser(out string errorMessage)
        { 
            errorMessage = null;
            CookieContainer cookieJar = GlobalAuthStore.CookieContainer;
            var URL_Prefix = Resources.URL_Prefix;

            try
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieJar
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{URL_Prefix}/auth/me"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting user failed: {response.StatusCode}";
                        return null;
                    }

                    var user = JsonConvert.DeserializeObject<AuthUserResponse>(json);

                    if (user == null)
                    {
                        errorMessage = "Invalid user response.";
                        return null;
                    }
                    return user;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        #endregion

    }
}
