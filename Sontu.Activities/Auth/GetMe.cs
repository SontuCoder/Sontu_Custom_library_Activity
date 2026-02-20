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

        [Category("Output")]
        [Description("Authentication error message.")]
        [DisplayName("Error")]
        public OutArgument<string> Error { get; set; }

        [Category("Output")]
        [Description("User details from DB.")]
        [DisplayName("User Details")]
        public OutArgument<AuthUserResponse> UserDetails { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            AuthUserResponse userDate = null;
            var cookies = GlobalAuthStore.CookieContainer;

            if (cookies == null)
            {
                Error.Set(context, "This activity must inside in Auth Scope or No Auth Scope.");
                UserDetails.Set(context, null);
                throw new InvalidOperationException("This activity must inside in Auth Scope or No Auth Scope.");
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

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = $"Getting user failed: {response.StatusCode}";
                        return null;
                    }

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
