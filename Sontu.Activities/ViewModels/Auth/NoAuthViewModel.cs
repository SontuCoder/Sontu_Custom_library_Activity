using System.Activities.DesignViewModels;using System.Net;

namespace Sontu.Activities.ViewModels.Auth
{
    public class NoAuthViewModel : DesignPropertiesViewModel
    {
        #region Design Properties

        public DesignInArgument<string> Email { get; set; }

        public DesignInArgument<CookieContainer> Cookie { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public NoAuthViewModel(IDesignServices services) : base(services)
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

            Cookie.DisplayName = "Cookies";
            Cookie.Tooltip = "The cookies after authentication.";
            Cookie.IsRequired = true;
            Cookie.IsPrincipal = true;
            Cookie.OrderIndex = 1;

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message if authentication fails.";
            Error.IsPrincipal = false;

        }
    }
}
