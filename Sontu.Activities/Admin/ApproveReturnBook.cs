using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Sontu.Activities.Admin
{
    public class ApproveReturnBook : CodeActivity
    {
        #region Properties
        public InArgument<string> IssuedId { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<APIResponse> ApprovalResponse { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string issuedId = IssuedId.Get(context);


            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ApprovalResponse.Set(context, null);
                throw new InvalidOperationException(msg);
            }


            // Input validation
            if (string.IsNullOrWhiteSpace(issuedId))
            {
                errorMessage = "Issued Id must be present.";
                Error.Set(context, errorMessage);
                ApprovalResponse.Set(context, null);
                throw new InvalidOperationException(errorMessage);
            }

            APIResponse approvalRes = ApprovalMethod(issuedId, out errorMessage);

            if (approvalRes == null)
            {
                Trace.TraceError($"Taking action on Issue failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ApprovalResponse.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Taking action on Issue succeeded.");
                ApprovalResponse.Set(context, approvalRes);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private APIResponse ApprovalMethod(string issuedId, out string errorMessage)
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
                        $"{URL_Prefix}/admin/approve-book-return-request/{issuedId}"
                    );

                    var content = new StringContent("", null, "text/plain");
                    request.Content = content;
                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Taking action on Issue failed: {response.StatusCode}";
                        return null;
                    }


                    var approvalRes = JsonConvert.DeserializeObject<APIResponse>(json);

                    if (approvalRes == null)
                    {
                        errorMessage = "Invalid API response.";
                        return null;
                    }
                    return approvalRes;
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
