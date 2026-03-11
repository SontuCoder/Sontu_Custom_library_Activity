using Newtonsoft.Json;
using Sontu.Activities.Helpers;
using Sontu.Activities.Models;
using System.Activities;
using System.Diagnostics;
using System.Net;

namespace Sontu.Activities.Admin
{
    public class GetAllStudents : CodeActivity
    {
        #region Properties
        public OutArgument<string> Error { get; set; }
        public OutArgument<ListOfStudents> ListOfStudents { get; set; }

        #endregion

        #region Execution
        protected override void Execute(CodeActivityContext context)
        {
            string errorMessage = null;

            if(!GlobalAuthStore.IsScopeActive)
            {
                var msg = "Activity must be used inside AuthScope.";
                Error.Set(context, msg);
                ListOfStudents.Set(context, null);
                throw new InvalidOperationException(msg);
            }

            ListOfStudents studentsList = GetListOfStudents(out errorMessage);

            if (studentsList == null)
            {
                Trace.TraceError($"Getting student details failed: {errorMessage}");
                Error.Set(context, errorMessage);
                ListOfStudents.Set(context, null);
            }
            else
            {
                Trace.TraceInformation($"Getting student details succeeded.");
                ListOfStudents.Set(context, studentsList);
                Error.Set(context, null);
            }
        }

        #endregion

        #region Private Functions

        private ListOfStudents GetListOfStudents(out string errorMessage)
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
                        $"{URL_Prefix}/admin/list-student"
                    );

                    var response = client.SendAsync(request).GetAwaiter().GetResult();

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorObj = JsonConvert.DeserializeObject<ApiErrorResponse>(json);

                        errorMessage = errorObj?.detail ?? $"Getting students failed: {response.StatusCode}";
                        return null;
                    }

                    var students = JsonConvert.DeserializeObject<ListOfStudents>(json);

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
