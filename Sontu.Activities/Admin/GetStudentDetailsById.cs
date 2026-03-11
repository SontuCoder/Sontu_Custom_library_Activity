using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetStudentDetailsById : CodeActivity
    {
        #region Properties
        public InArgument<string> StudentId { get; set; }
        public OutArgument<string> Error { get; set; }
        public OutArgument<StudentDetails> StudentDetails { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;
            string studentId = StudentId.Get(context);

            if (!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                StudentDetails.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            StudentDetails studentDetails = GetStudentDetails(studentId, out errorMessage);

            if (studentDetails == null)
            {
                Trace.TraceError($"Getting student details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                StudentDetails.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting student details succeeded.");
                StudentDetails.Set(context, studentDetails);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private StudentDetails GetStudentDetails(string studentId, out string errorMessage)
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
                        $"{URL_Prefix}/admin/student-details/{studentId}"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting students failed: {response.StatusCode}";
                        return null;
                    }

                    var students = JsonConvert.DeserializeObject<StudentResponse>(json).Data;

                    if (students == null)
                    {
                        errorMessage = "Invalid student response.";
                        return null;
                    }
                    return students;
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
