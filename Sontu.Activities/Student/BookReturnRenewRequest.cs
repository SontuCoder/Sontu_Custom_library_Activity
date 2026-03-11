using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sontu.Activities.Student
{
    public class BookReturnRenewRequest : CodeActivity
    {

        #region Properties
        public InArgument<string> BookId { get; set; }
        public InArgument<StudentRequestAction> Action { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<APIResponse> RequestBookResponse { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string bookId = BookId.Get(context);
            var action = Action.Get(context);
            string actionStr = action == StudentRequestAction.Return ? "return" : "renew";

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                RequestBookResponse.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            // Input validation
            if (string.IsNullOrEmpty(bookId))
            {
                errorMessage = "BookId is required.";
                Trace.TraceError($"Request book failed: {errorMessage}");
                Error.Set(context, errorMessage);
                RequestBookResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (action != StudentRequestAction.Return && action != StudentRequestAction.Renew)
            {
                errorMessage = "Invalid action. Must be Return or Renew.";
                Trace.TraceError($"Request book failed: {errorMessage}");
                Error.Set(context, errorMessage);
                RequestBookResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            APIResponse apiDate = ReqBook(bookId, actionStr, out errorMessage);

            if (apiDate == null)
            {
                Trace.TraceError($"Request book failed: {errorMessage}");
                Error.Set(context, errorMessage);
                RequestBookResponse.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Request book succeeded.");
                RequestBookResponse.Set(context, apiDate);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Authentication

        private APIResponse ReqBook(string bookId, string actionStr, out string errorMessage)
        { 
            errorMessage = null;
            string userEmail = GlobalAuthStore.UserEmail;
            GlobalAuthStore.CookieContainer.TryGetValue(userEmail.ToLower(), out var cookieJar);
            var URL_Prefix = Resources.URL_Prefix;

            if(actionStr == "return")
            {
                URL_Prefix += "/student/return-book";
            }
            else if(actionStr == "renew")
            {
                URL_Prefix += "/student/book-renew-request";
            }

            try
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieJar
                };
                Console.WriteLine($"Debug: {URL_Prefix}/{bookId}");

                using (var client = new HttpClient(handler))
                {
                    var request = new HttpRequestMessage(
                        HttpMethod.Post,
                        $"{URL_Prefix}/{bookId}"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Taking action on book failed: {response.StatusCode}";
                        return null;
                    }

                    var bookRes = JsonConvert.DeserializeObject<APIResponse>(json);

                    if (bookRes == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return bookRes;
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
