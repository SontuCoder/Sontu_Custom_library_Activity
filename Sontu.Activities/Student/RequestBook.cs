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
    public class RequestBook : CodeActivity
    {

        #region Properties
        public InArgument<string> BookId { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<APIResponse> RequestBookResponse { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string bookId = BookId.Get(context);

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

            APIResponse apiDate = ReqBook(bookId, out errorMessage);

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

        private APIResponse ReqBook(string bookId, out string errorMessage)
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
                        HttpMethod.Post,
                        $"{URL_Prefix}/student/book-request"
                    );

                    var bookData = new
                    {
                        book_id = bookId
                    };

                    var jsonData = JsonConvert.SerializeObject(bookData);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    request.Content = content;

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Request book failed: {response.StatusCode}";
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
