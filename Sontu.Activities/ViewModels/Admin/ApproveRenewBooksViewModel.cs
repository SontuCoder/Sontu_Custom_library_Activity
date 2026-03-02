using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class ApproveRenewBooksViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<string> IssuedId { get; set; }
        public DesignOutArgument<string> Error { get; set; }
        public DesignInArgument<ApprovalAction> Action { get; set; }
        public DesignOutArgument<APIResponse> ApprovalResponse { get; set; }

        #endregion

        public ApproveRenewBooksViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;
            Error.OrderIndex = 2;

            ApprovalResponse.DisplayName = "RenewResponse";
            ApprovalResponse.Tooltip = "The details of the response after action taken.";
            ApprovalResponse.IsPrincipal = false;
            ApprovalResponse.OrderIndex = 1;

            IssuedId.DisplayName = "Issue Id";
            IssuedId.Tooltip = "The issue Id for taking action.";
            IssuedId.IsPrincipal = true;
            IssuedId.IsRequired = true;
            IssuedId.OrderIndex = 0;

            Action.DisplayName = "Action";
            Action.Tooltip = "The action to take.";
            Action.IsPrincipal = true;
            Action.IsRequired = true;
            Action.OrderIndex = 1;
        }
    }
}
