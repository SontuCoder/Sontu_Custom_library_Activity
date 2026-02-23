using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Book
{
    public class GetAllBooks : CodeActivity
    {
        #region Properties
        public InArgument<int?> Limit { get; set; }
        public InArgument<string> Cursor { get; set; }
        public InArgument<string> BookType { get; set; }
        public InArgument<string> BookName { get; set; }
        public InArgument<string> BookAuthor { get; set; }
        public InArgument<int?> BookEdition { get; set; }

        public OutArgument<string> Error { get; set; }
        public OutArgument<GetBooks> BookDetails { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            GetBooks bookDetails = null;
            int? limit = Limit.Get(context);
            string cursor = Cursor.Get(context);
            string bookType = BookType.Get(context);
            string bookName = BookName.Get(context);
            string bookAuthor = BookAuthor.Get(context);
            int? bookEdition = BookEdition.Get(context);


            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                BookDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            if ( limit.HasValue && (limit < 0 || limit > 100))
            {
                var msg = "Limit must be between 0 to 100.";
                Error.Set(context, msg);
                BookDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            if (bookEdition.HasValue && bookEdition < 0)
            {
                var msg = "Edition must be greater than 0.";
                Error.Set(context, msg);
                BookDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            string condition = null;
            if (!limit.HasValue) {
                condition = "?limit=20";
            }
            else
            {
                condition = "?limit="+limit.ToString();
            }

            if (cursor != null)
                condition = condition + "&cursor=" + cursor.Trim();

            if(bookType != null)
            {
                foreach (var item in bookType.Split(","))
                {
                    condition = condition + "&book_type=" + item.Trim();
                }
            }

            if (bookName != null)
                condition = condition + "&book_name=" + bookName.Trim();

            if (bookAuthor != null)
                condition = condition + "&book_author=" + bookAuthor.Trim();

            if( bookEdition.HasValue)
                condition = condition + "&edition=" + bookEdition;

            bookDetails = GetBooksData(condition, out errorMessage);

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

        private GetBooks GetBooksData(string condition, out string errorMessage)
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
                        HttpMethod.Get,
                        $"{URL_Prefix}/books/all{condition}"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = $"Getting books failed: {response.StatusCode}";
                        return null;
                    }

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    var book = JsonConvert.DeserializeObject<GetBooks>(json);

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
