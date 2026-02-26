using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetAllAdmins : CodeActivity
    {
        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<AllAdmins> ListOfAdmins { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ListOfAdmins.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            AllAdmins allAdmins = GetListOfAdmins(out errorMessage);

            if (allAdmins == null)
            {
                Trace.TraceError($"Getting admins details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ListOfAdmins.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting admins details succeeded.");
                ListOfAdmins.Set(context, allAdmins);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private AllAdmins GetListOfAdmins(out string errorMessage)
        { 
            errorMessage = null;
            CookieContainer cookieJar = GlobalAuthStore.CookieContainer;
            var URL_Prefix = Resources.URL_Prefix;

            try
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieJar,
                    UseCookies = true
                };

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{URL_Prefix}/admin/get-all-admins"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting admins details failed: {response.StatusCode}";
                        return null;
                    }

                    var allAdmins = JsonConvert.DeserializeObject<AllAdmins>(json);

                    if (allAdmins == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return allAdmins;
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
