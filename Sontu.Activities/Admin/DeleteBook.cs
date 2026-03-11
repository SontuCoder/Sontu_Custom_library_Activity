using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sontu.Activities.Admin
{
    public class DeleteBook: CodeActivity
    {
        #region Properties
        public InArgument<string> BookId { get; set; }
        public InArgument<int> Quantity { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<DeleteBookResponse> DeleteBookRes { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string bookid = BookId.Get(context);
            int quantity = Quantity.Get(context);

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                DeleteBookRes.Set(context, null);
                throw new InvalidOperationException(msg);
            }


            // Input validation
            if(string.IsNullOrWhiteSpace(bookid)) {
                errorMessage = "Book id must be present.";
                Error.Set(context, errorMessage);
                DeleteBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (quantity <= 0)
            {
                errorMessage = "Quantity must be greater than 0.";
                Error.Set(context, errorMessage);
                DeleteBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            DeleteBookResponse deleteBookRes = DeleteBookMethod(bookid, quantity, out errorMessage);

            if (deleteBookRes == null)
            {
                Trace.TraceError($"Deleting book failed: {errorMessage}");
                Error.Set(context, errorMessage);
                DeleteBookRes.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Deleting book succeeded.");
                DeleteBookRes.Set(context, deleteBookRes);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private DeleteBookResponse DeleteBookMethod(string bookId, int quantity, out string errorMessage)
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
                        HttpMethod.Delete,
                        $"{URL_Prefix}/admin/delete-book"
                    );

                    var bookData = new
                    {
                        id=bookId,
                        quantity,
                    };

                    var jsonData = JsonConvert.SerializeObject(bookData);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    request.Content = content;
                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Deleting book failed: {response.StatusCode}";
                        return null;
                    }

                    var delbook = JsonConvert.DeserializeObject<DeleteBookResponse>(json);

                    if (delbook == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return delbook;
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
