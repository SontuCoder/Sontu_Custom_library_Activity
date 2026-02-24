using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class DeleteBookViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<string> BookId { get; set; }
        public DesignInArgument<int> Quantity { get; set; }
        public DesignOutArgument<DeleteBookResponse> DeleteBookRes { get; set; }
        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public DeleteBookViewModel(IDesignServices services) : base(services)
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

            DeleteBookRes.DisplayName = "DeletedBookResponse";
            DeleteBookRes.Tooltip = "The details of the deleted book.";
            DeleteBookRes.IsPrincipal = false;
            DeleteBookRes.OrderIndex = 1;

            BookId.DisplayName = "Book Id";
            BookId.Tooltip = "The book Id for delete.";
            BookId.IsPrincipal = true;
            BookId.IsRequired = true;
            BookId.OrderIndex = 0;

            Quantity.DisplayName = "Quantity";
            Quantity.Tooltip = "The books quantity to be deleted.";
            Quantity.IsPrincipal = true;
            Quantity.IsRequired = true;
            Quantity.OrderIndex = 1;
        }
    }
}
