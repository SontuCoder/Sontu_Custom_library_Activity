using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetAllIssuedBooks : CodeActivity
    {
        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<ListOfIssuedBooks> ListOfIssuedBooks{ get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ListOfIssuedBooks.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            ListOfIssuedBooks issuedBooksList = GetListOfIssuedBooks(out errorMessage);

            if (issuedBooksList == null)
            {
                Trace.TraceError($"Getting issued books details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ListOfIssuedBooks.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting issued books details succeeded.");
                ListOfIssuedBooks.Set(context, issuedBooksList);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private ListOfIssuedBooks GetListOfIssuedBooks(out string errorMessage)
        { 
            errorMessage = null;
            string userEmail = GlobalAuthStore.UserEmail;
            GlobalAuthStore.CookieContainer.TryGetValue(userEmail.ToLower(), out var cookieJar);
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
                        $"{URL_Prefix}/admin/issued-books"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting issued books failed: {response.StatusCode}";
                        return null;
                    }


                    var issuedBooks = JsonConvert.DeserializeObject<ListOfIssuedBooks>(json);

                    if (issuedBooks == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return issuedBooks;
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
