using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Net.Http;

namespace Sontu.Activities.Student
{
    public class GetAllRequests : CodeActivity
    {

        #region Properties
        public InArgument<StudentRequestStatus> RequestStatus { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<ListOfStudentRequests> AllRequests { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;


            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                AllRequests.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            // Input validation
            if (RequestStatus.Get(context) != StudentRequestStatus.All && RequestStatus.Get(context) != StudentRequestStatus.Issued && RequestStatus.Get(context) != StudentRequestStatus.Returned)
            {
                var msg = "Invalid RequestStatus. Please provide a valid status.";
                Error.Set(context, msg);
                AllRequests.Set(context, null);
                throw new ArgumentException(msg, nameof(RequestStatus));
            }

            var status = RequestStatus.Get(context);
            var statusStr = status.ToString();

            ListOfStudentRequests apiDate = AllRequestsBooks(statusStr, out errorMessage);

            if (apiDate == null)
            {
                Trace.TraceError($"Get all requests failed: {errorMessage}");
                Error.Set(context, errorMessage);
                AllRequests.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Get all requests succeeded.");
                AllRequests.Set(context, apiDate);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Authentication

        private ListOfStudentRequests AllRequestsBooks(string status, out string errorMessage)
        { 
            errorMessage = null;
            string userEmail = GlobalAuthStore.UserEmail;
            GlobalAuthStore.CookieContainer.TryGetValue(userEmail.ToLower(), out var cookieJar);
            var URL_Prefix = Resources.URL_Prefix;
            if(status.ToLower() == "issued")
            {
                URL_Prefix += "/student/issued-books";
            } else if(status.ToLower() == "all") {
                URL_Prefix += "/student/my-requests";
            } else
            {
                URL_Prefix += "/student/list-return-request";
            }

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
                            $"{URL_Prefix}"
                        );

                        var response = client.SendAsync(request).GetAwaiter().GetResult();

                        var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (!response.IsSuccessStatusCode)
                        {
                            var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                            errorMessage = errorObj?.detail ?? $"Get all requests failed: {response.StatusCode}";
                            return null;
                        }

                        var bookRes = JsonConvert.DeserializeObject<ListOfStudentRequests>(json);

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
