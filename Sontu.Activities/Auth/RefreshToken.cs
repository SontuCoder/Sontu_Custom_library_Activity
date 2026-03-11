using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Auth
{
    public class RefreshToken : CodeActivity
    {
        #region Constants


        #endregion

        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<CookieContainer> NewCookie { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            CookieContainer newCookie = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                NewCookie.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            newCookie = RefreshTokenCall(out errorMessage);

            if (newCookie == null)
            {
                Trace.TraceError($"Refreshing token failed: {errorMessage}");
                Error.Set(context, errorMessage);
                NewCookie.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Refresh token succeeded.");
                NewCookie.Set(context, newCookie);
                Error.Set(context, null);
                string userEmail = GlobalAuthStore.UserEmail;
                GlobalAuthStore.CookieContainer[userEmail] = newCookie;
            }
        }

        #endregion

        #region Private Authentication

        private CookieContainer RefreshTokenCall(out string errorMessage)
        { 
            errorMessage = null;
            string userEmail = GlobalAuthStore.UserEmail;
            GlobalAuthStore.CookieContainer.TryGetValue(userEmail.ToLower(), out var cookieJar);
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
                        HttpMethod.Post,
                        $"{URL_Prefix}/auth/refresh-token"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Refresh token failed: {response.StatusCode}";
                        return null;
                    }

                }
                return cookieJar;
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
