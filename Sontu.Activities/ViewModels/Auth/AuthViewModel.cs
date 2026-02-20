using System.Activities.DesignViewModels;using System.Net;

namespace Sontu.Activities.ViewModels.Auth
{
    public class AuthViewModel : DesignPropertiesViewModel
    {
        #region Design Properties

        public DesignInArgument<string> Email { get; set; }

        public DesignInArgument<string> Password { get; set; }

        public DesignOutArgument<CookieContainer> Cookie { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public AuthViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Email.DisplayName = "Email";
            Email.Tooltip = "The email ID for authentication.";
            Email.IsRequired = true;
            Email.IsPrincipal = true;
            Email.OrderIndex = 0;

            Password.DisplayName = "Password";
            Password.Tooltip = "The password for authentication.";
            Password.IsRequired = true;
            Password.IsPrincipal = true;
            Password.OrderIndex = 1;

            Cookie.DisplayName = "Cookies";
            Cookie.Tooltip = "The cookies after authentication.";
            Cookie.IsPrincipal = false;

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message if authentication fails.";
            Error.IsPrincipal = false;

        }
    }
}
