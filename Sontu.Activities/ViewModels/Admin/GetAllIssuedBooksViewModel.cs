using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class GetAllIssuedBooksViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<ListOfIssuedBooks> ListOfIssuedBooks { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetAllIssuedBooksViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;

            ListOfIssuedBooks.DisplayName = "ListOfIssuedBooks";
            ListOfIssuedBooks.Tooltip = "The list of all issued books details.";
            ListOfIssuedBooks.IsPrincipal = true;
            ListOfIssuedBooks.OrderIndex = 0;

        }
    }
}
