using Sontu.Activities.Models;
using System.Activities.DesignViewModels;
using System.Net;

namespace Sontu.Activities.ViewModels.Auth
{
    public class RefreshTokenViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<CookieContainer> NewCookie { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public RefreshTokenViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message if authentication fails.";
            Error.IsPrincipal = false;

            NewCookie.DisplayName = "NewCookie";
            NewCookie.Tooltip = "The new token if authentication success.";
            NewCookie.IsPrincipal = true;
            NewCookie.OrderIndex = 0;

        }
    }
}
