using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetAllRenewReturnReqBooks : CodeActivity
    {
        #region Properties
        public InArgument<RequestOptions> RequestOption { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<RequestBookResponse> ListOfRenewReqBooks { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            RequestOptions option = RequestOption.Get(context);
            string optionStr = option.ToString();

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ListOfRenewReqBooks.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            if (!Enum.IsDefined(typeof(RequestOptions), option))
            {
                errorMessage = "Invalid option.";
                Error.Set(context, errorMessage);
                ListOfRenewReqBooks.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            RequestBookResponse renewBooksList = GetListOfRenewReqBooks(optionStr, out errorMessage);

            if (renewBooksList == null)
            {
                Trace.TraceError($"Getting requested books details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ListOfRenewReqBooks.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting requested books details succeeded.");
                ListOfRenewReqBooks.Set(context, renewBooksList);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private RequestBookResponse GetListOfRenewReqBooks(string option, out string errorMessage)
        {
            errorMessage = null;
            CookieContainer cookieJar = GlobalAuthStore.CookieContainer;
            var URL_Prefix = Resources.URL_Prefix;
            var requestUri = "";
            if (option == "Renew")
            {
                requestUri = $"{URL_Prefix}/admin/list-books-renew-requested";
            } else
            {
                requestUri = $"{URL_Prefix}/admin/list-books-return-requested";
            }

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
                            requestUri
                        );

                        var response = client.SendAsync(request).GetAwaiter().GetResult();

                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                            errorMessage = errorObj?.detail ?? $"Getting requested books failed: {response.StatusCode}";
                            return null;
                        }


                        var issuedBooks = JsonConvert.DeserializeObject<RequestBookResponse>(json);

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
