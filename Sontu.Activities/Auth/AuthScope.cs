using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace Sontu.Activities.Auth
{
    public class AuthScope : NativeActivity
    {
        #region Constants


        #endregion

        #region Properties

        public InArgument<string> Email { get; set; }
        public InArgument<string> Password { get; set; }
        public OutArgument<CookieContainer> Cookie { get; set; }
        public OutArgument<string> Error { get; set; }

        [Browsable(false)]
        public Activity Body { get; set; }


        #endregion

        #region Execution

        protected override void Execute(NativeActivityContext context)
        {
            string email = Email.Get(context);
            string password = Password.Get(context);

            CookieContainer cookieJar = null;
            string errorMessage = null;

            try
            {
                cookieJar = Authenticate(email, password, out errorMessage);

                if (cookieJar == null)
                {
                    errorMessage ??= "Authentication failed. No cookie generated.";
                    Error.Set(context, errorMessage);
                    Cookie.Set(context, null);
                    throw new InvalidOperationException(errorMessage);
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                Error.Set(context, errorMessage);
                Cookie.Set(context, null);

                throw new InvalidOperationException(errorMessage);
            }

            Error.Set(context, null);
            Cookie.Set(context, cookieJar);

            if (cookieJar != null)
            {
                GlobalAuthStore.CookieContainer = cookieJar;
                GlobalAuthStore.IsScopeActive = true;
                GlobalAuthStore.UserEmail = email;
            }
            if (Body != null)
                context.ScheduleActivity( Body, OnBodyCompleted );
        }

        #endregion

        #region Private Authentication

        private CookieContainer Authenticate(string email, string password, out string errorMessage)
        {
            errorMessage = null;
            var cookieJar = new CookieContainer();
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
                        $"{URL_Prefix}/auth/login"
                    );

                    string jsonBody = $@"{{
                        ""email"": ""{email}"",
                        ""password"": ""{password}""
                    }}";

                    request.Content = new StringContent(
                        jsonBody,
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Authentication failed: {response.StatusCode}";
                        return null;
                    }

                    // Verify cookie received
                    var uri = new Uri(URL_Prefix);
                    var cookies = cookieJar.GetCookies(uri);

                    if (cookies.Count == 0)
                    {
                        errorMessage = "Authentication failed: No cookies returned.";
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


        private void OnBodyCompleted( NativeActivityContext context, ActivityInstance completedInstance)
        {
            GlobalAuthStore.IsScopeActive = false;
        }

        #endregion

    }
}


