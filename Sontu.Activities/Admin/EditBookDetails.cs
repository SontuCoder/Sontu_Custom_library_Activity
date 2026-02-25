using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sontu.Activities.Admin
{
    public class EditBookDetails : CodeActivity
    {
        #region Properties
        public InArgument<string> BookId { get; set; }
        public InArgument<string> Title { get; set; }
        public InArgument<string> Author { get; set; }
        public InArgument<string> Description { get; set; }
        public InArgument<int?> Edition { get; set; }
        public InArgument<string> Categories { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<APIResponse> EditBookResponse { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string title = Title.Get(context)?.Trim();
            string author = Author.Get(context)?.Trim();
            string description = Description.Get(context)?.Trim();
            int? edition = Edition.Get(context);
            string categories = Categories.Get(context)?.Trim();
            string bookId = BookId.Get(context)?.Trim();
            List<string> category = new List<string>();

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                EditBookResponse.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            // Input validation
            if (string.IsNullOrWhiteSpace(bookId))
            {
                errorMessage = "Book Id must be present.";
                Error.Set(context, errorMessage);
                EditBookResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (!string.IsNullOrWhiteSpace(categories))
            {
                foreach (var cat in categories.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(cat))
                    {
                        category.Add(cat.Trim());
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(description) || category.Count==0 || !edition.HasValue)
            {
                errorMessage = "Input values are Missing.";
                Error.Set(context, errorMessage);
                EditBookResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }


            APIResponse editBookRes = EditBookDetailsMethod(bookId, title, author, description, category, edition, out errorMessage);

            if (editBookRes == null)
            {
                Trace.TraceError($"Edit book details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                EditBookResponse.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Edit book details succeeded.");
                EditBookResponse.Set(context, editBookRes);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private APIResponse EditBookDetailsMethod(string id, string title, string author, string description, List<string> category, int? edition, out string errorMessage)
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
                        HttpMethod.Post,
                        $"{URL_Prefix}/admin/book-details-edit"
                    );

                    var NewBookData = new
                    {
                        id,
                        title,
                        author,
                        description,
                        edition,
                        category
                    };

                    var jsonData = JsonConvert.SerializeObject(NewBookData);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    request.Content = content;
                    var response = client.SendAsync(request).GetAwaiter().GetResult();


                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Edit book details failed: {response.StatusCode}";
                        return null;
                    }

                    var editBookRes = JsonConvert.DeserializeObject<APIResponse>(json);

                    if (editBookRes == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return editBookRes;
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
