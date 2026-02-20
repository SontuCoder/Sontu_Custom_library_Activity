using Sontu.Activities.Helpers;
using System.Activities;
using System.ComponentModel;
using System.Net;
using System.Text;
using UiPath.Robot.Activities.Api;

namespace Sontu.Activities.Auth
{
    public class AuthScope : NativeActivity
    {
        #region Constants


        #endregion

        #region Properties

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Email")]
        [Description("The email ID for authentication.")]
        public InArgument<string> Email { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Password")]
        [Description("The password for authentication.")]
        public InArgument<string> Password { get; set; }

        [Category("Output")]
        [Description("Authentication Cookie.")]
        [DisplayName("Cookies")]
        public OutArgument<CookieContainer> Cookie { get; set; }

        [Category("Output")]
        [Description("Authentication error message.")]
        [DisplayName("Error")]
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
            }
            if (Body != null)
                context.ScheduleActivity(Body);
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

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = $"Authentication failed: {response.StatusCode}";
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

        #endregion

    }
}


