using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class GetAllRenewReturnReqBooksViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<RequestOptions> RequestOption { get; set; }
        public DesignOutArgument<RequestBookResponse> ListOfRenewReqBooks { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetAllRenewReturnReqBooksViewModel(IDesignServices services) : base(services)
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

            ListOfRenewReqBooks.DisplayName = "ListOfRenewReqBooks";
            ListOfRenewReqBooks.Tooltip = "The list of all requested books details.";
            ListOfRenewReqBooks.IsPrincipal = false;
            ListOfRenewReqBooks.OrderIndex = 0;

            RequestOption.DisplayName = "ListOfRenewReqBooks";
            RequestOption.Tooltip = "The Option to get books details.";
            RequestOption.IsPrincipal = true;
            RequestOption.IsRequired = true;

        }
    }
}
