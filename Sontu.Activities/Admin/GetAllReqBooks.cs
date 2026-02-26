using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetAllReqBooks: CodeActivity
    {
        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<ListOfRequestedBook> ListOfReqBooks { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ListOfReqBooks.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            ListOfRequestedBook requestList = GetListOfRequests(out errorMessage);

            if (requestList == null)
            {
                Trace.TraceError($"Getting book request details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ListOfReqBooks.Set(context, null);
            }
            else { 
                Trace.TraceInformation($"Getting book request details succeeded.");
                ListOfReqBooks.Set(context, requestList);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private ListOfRequestedBook GetListOfRequests(out string errorMessage)
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
                        $"{URL_Prefix}/admin/list-books-requested"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting book request failed: {response.StatusCode}";
                        return null;
                    }

                    var reqbooks = JsonConvert.DeserializeObject<ListOfRequestedBook>(json);

                    if (reqbooks == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return reqbooks;
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
