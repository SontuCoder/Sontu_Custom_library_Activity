using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Student
{
    public class BookReturnRenewRequestViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<APIResponse> RequestBookResponse { get; set; }
        public DesignInArgument<string> BookId { get; set; }
        public DesignInArgument<StudentRequestAction> Action { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public BookReturnRenewRequestViewModel(IDesignServices services) : base(services)
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

            RequestBookResponse.DisplayName = "RequestBookResponse";
            RequestBookResponse.Tooltip = "Api response for requesting book.";
            RequestBookResponse.IsPrincipal = false;
            RequestBookResponse.OrderIndex = 0;

            BookId.DisplayName = "BookId";
            BookId.Tooltip = "The book id for requesting.";
            BookId.IsPrincipal = true;
            BookId.IsRequired = true;
            BookId.OrderIndex = 0;

            Action.DisplayName = "Action";
            Action.Tooltip = "The action for the return or renew.";
            Action.IsPrincipal = true;
            Action.IsRequired = true;
            Action.OrderIndex = 1;

        }
    }
}
