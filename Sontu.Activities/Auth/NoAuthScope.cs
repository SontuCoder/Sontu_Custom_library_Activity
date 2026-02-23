using Sontu.Activities.Helpers;
using System.Activities;
using System.ComponentModel;
using System.Net;

namespace Sontu.Activities.Auth
{
    public class NoAuthScope : NativeActivity
    {
        #region Constants


        #endregion

        #region Properties

        public InArgument<CookieContainer> Cookie { get; set; }
        public InArgument<string> Email { get; set; }
        public OutArgument<string> Error { get; set; }

        [Browsable(false)]
        public Activity Body { get; set; }


        #endregion

        #region Execution

        protected override void Execute(NativeActivityContext context)
        {
            string email = Email.Get(context);
            CookieContainer cookies = Cookie.Get(context);

            string errorMessage = null;

            if (GlobalAuthStore.IsScopeActive)
            {
                errorMessage = "An authentication scope is already active. Please ensure that scopes are properly nested and not overlapping.";
                Error.Set(context, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if(GlobalAuthStore.CookieContainer == null || GlobalAuthStore.UserEmail == null)
            {
                errorMessage = "First run auth scope.";
                Error.Set(context, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if (email != GlobalAuthStore.UserEmail || !ReferenceEquals(cookies, GlobalAuthStore.CookieContainer))
            {
                errorMessage = "User details do not match the active session.";
                Error.Set(context, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            Error.Set(context, null);
            GlobalAuthStore.IsScopeActive = true;

            if (Body != null)
                context.ScheduleActivity( Body, OnBodyCompleted );
        }

        #endregion

        #region Private Authentication

        private void OnBodyCompleted( NativeActivityContext context, ActivityInstance completedInstance)
        {
            GlobalAuthStore.IsScopeActive = false;
        }

        #endregion

    }
}


