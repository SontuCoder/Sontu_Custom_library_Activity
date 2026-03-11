using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sontu.Activities.Admin
{
    public class AddNewBook: CodeActivity
    {
        #region Properties
        public InArgument<string> Title { get; set; }
        public InArgument<string> Author { get; set; }
        public InArgument<string> Description { get; set; }
        public InArgument<string> Category { get; set; }
        public InArgument<int> Edition { get; set; }
        public InArgument<int> Quantity { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<AddBookResponse> AddBookRes { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string title = Title.Get(context);
            string author = Author.Get(context);
            string description = Description.Get(context);
            string category = Category.Get(context);
            int edition = Edition.Get(context);
            int quantity = Quantity.Get(context);
            List<string> categories = new List<string>();
            List<string> validCategories = new List<string> { "mathematics", "physics", "computer science", "literature", "others" };


            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(msg);
            }


            // Input validation
            if(string.IsNullOrWhiteSpace(title)) {
                errorMessage = "Title must be present.";
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                errorMessage = "Auther must be present.";
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage); ;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                errorMessage = "Description must be present.";
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                foreach(var cat in category.Split(','))
                { 
                    if (!string.IsNullOrWhiteSpace(cat.Trim()))
                    {
                        if(validCategories.Any(x => x == cat.Trim()))
                        {
                            categories.Add(cat.Trim());
                        } else
                        {
                            errorMessage = $"{cat} is a wrong category.";
                            Error.Set(context, errorMessage);
                            AddBookRes.Set(context, null);
                            throw new InvalidOperationException(errorMessage);
                        }
                        
                    }
                }
            }
            if (edition <= 0)
            {
                errorMessage = "Edition must be greater than 0.";
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (quantity <= 0)
            {
                errorMessage = "Quantity must be greater than 0.";
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            AddBookResponse addNewBookRes = AddBook(title, author, description, categories, edition, quantity,out errorMessage);

            if (addNewBookRes == null)
            {
                Trace.TraceError($"Adding new book failed: {errorMessage}");
                Error.Set(context, errorMessage);
                AddBookRes.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Adding new book succeeded.");
                AddBookRes.Set(context, addNewBookRes);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private AddBookResponse AddBook(string title, string author, string description, List<string> categories, int edition, int quantity, out string errorMessage)
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
                        HttpMethod.Post,
                        $"{URL_Prefix}/admin/add-book"
                    );

                    var bookData = new
                    {
                        title,
                        author,
                        description,
                        edition,
                        quantity,
                        category = categories
                    };

                    var jsonData = JsonConvert.SerializeObject(bookData);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    request.Content = content;
                    var response = client.SendAsync(request).GetAwaiter().GetResult();


                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Adding new book failed: {response.StatusCode}";
                        return null;
                    }

                    var newbook = JsonConvert.DeserializeObject<AddBookResponse>(json);

                    if (newbook == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return newbook;
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
