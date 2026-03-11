using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Student
{
    public class DeleteIssueViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<APIResponse> DeleteIssueResponse { get; set; }
        public DesignInArgument<string> RequestId { get; set; }
        public DesignInArgument<StudentRequestFilter> Action { get; set; }
        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public DeleteIssueViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;
            Error.OrderIndex = 1;

            DeleteIssueResponse.DisplayName = "DeleteIssueResponse";
            DeleteIssueResponse.Tooltip = "Api response for deleting issue.";
            DeleteIssueResponse.IsPrincipal = false;
            DeleteIssueResponse.OrderIndex = 0;

            RequestId.DisplayName = "RequestId";
            RequestId.Tooltip = "The issue id for deleting.";
            RequestId.IsPrincipal = true;
            RequestId.IsRequired = true;
            RequestId.OrderIndex = 0;

            Action.DisplayName = "Action";
            Action.Tooltip = "The action for the return or renew or request.";
            Action.IsPrincipal = true;
            Action.IsRequired = true;
            Action.OrderIndex = 1;

        }
    }
}
