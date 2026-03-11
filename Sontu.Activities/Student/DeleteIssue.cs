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
    public class DeleteIssue : CodeActivity
    {

        #region Properties
        public InArgument<string> RequestId { get; set; }
        public InArgument<StudentRequestFilter> Action { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<APIResponse> DeleteIssueResponse { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string issueId = RequestId.Get(context);
            var action = Action.Get(context);
            string actionStr = action.ToString() ;

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                DeleteIssueResponse.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            // Input validation
            if (string.IsNullOrEmpty(issueId))
            {
                errorMessage = "IssueId is required.";
                Trace.TraceError($"Delete issue failed: {errorMessage}");
                Error.Set(context, errorMessage);
                DeleteIssueResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }
            if (action != StudentRequestFilter.Return && action != StudentRequestFilter.Renew && action != StudentRequestFilter.Request)
            {
                errorMessage = "Invalid action. Must be 'Request' or 'Renew' or 'Return'.";
                Trace.TraceError($"Delete issue failed: {errorMessage}");
                Error.Set(context, errorMessage);
                DeleteIssueResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            APIResponse apiDate = DeletIssue(issueId, actionStr, out errorMessage);

            if (apiDate == null)
            {
                Trace.TraceError($"Delete issue failed: {errorMessage}");
                Error.Set(context, errorMessage);
                DeleteIssueResponse.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Delete issue succeeded.");
                DeleteIssueResponse.Set(context, apiDate);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Authentication

        private APIResponse DeletIssue(string issueId, string action, out string errorMessage)
        { 
            errorMessage = null;
            string userEmail = GlobalAuthStore.UserEmail;
            GlobalAuthStore.CookieContainer.TryGetValue(userEmail.ToLower(), out var cookieJar);
            var URL_Prefix = Resources.URL_Prefix;
            if (action == StudentRequestFilter.Request.ToString())
            {
                URL_Prefix += "/student/delete-issued-request";
            }
             else if (action == StudentRequestFilter.Renew.ToString())
            {
                URL_Prefix += "/student/delete-renew-request";
            }
             else if (action == StudentRequestFilter.Return.ToString())
            {
                URL_Prefix += "/student/delete-return-request";
            }
             else
            {
                errorMessage = "Invalid action. Must be 'Request' or 'Renew' or 'Return'.";
                return null;
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
                        HttpMethod.Post,
                        $"{URL_Prefix}/{issueId}"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Delete issue failed: {response.StatusCode}";
                        return null;
                    }

                    var apiRes = JsonConvert.DeserializeObject<APIResponse>(json);

                    if (apiRes == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return apiRes;
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
