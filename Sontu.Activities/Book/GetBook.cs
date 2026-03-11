using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Book
{
    public class GetBook : CodeActivity
    {
        #region Properties
        public InArgument<string> BookId { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<GetBookDetails> BookDetails { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            GetBookDetails bookDetails = null;
            string bookId = BookId.Get(context);

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                BookDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            bookDetails = GetBookDetails(bookId,out errorMessage);

            if (bookDetails == null)
            {
                Trace.TraceError($"Getting book details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                BookDetails.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting book details succeeded.");
                BookDetails.Set(context, bookDetails);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private GetBookDetails GetBookDetails(string book_id, out string errorMessage)
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
                        HttpMethod.Get,
                        $"{URL_Prefix}/books/details/{book_id}"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = $"Getting book failed: {response.StatusCode}";
                        return null;
                    }

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    var book = JsonConvert.DeserializeObject<GetBookResponse>(json).Data;

                    if (book == null)
                    {
                        errorMessage = "Invalid book response.";
                        return null;
                    }
                    return book;
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
