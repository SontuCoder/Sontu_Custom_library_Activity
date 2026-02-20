using Sontu.Activities.Models;
using System.Activities.DesignViewModels;
using System.Net;

namespace Sontu.Activities.ViewModels.Auth
{
    public class GetMeViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<AuthUserResponse> UserDetails { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetMeViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message if authentication fails.";
            Error.IsPrincipal = false;

            UserDetails.DisplayName = "User Details";
            UserDetails.Tooltip = "The user details if authentication success.";
            UserDetails.IsPrincipal = true;
            UserDetails.OrderIndex = 0;

        }
    }
}
