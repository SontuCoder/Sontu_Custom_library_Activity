using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class GetAllReqBooksViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<ListOfRequestedBook> ListOfReqBooks { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetAllReqBooksViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;

            ListOfReqBooks.DisplayName = "ListOfReqBooks";
            ListOfReqBooks.Tooltip = "The list of all requested books details.";
            ListOfReqBooks.IsPrincipal = true;
            ListOfReqBooks.OrderIndex = 0;

        }
    }
}
