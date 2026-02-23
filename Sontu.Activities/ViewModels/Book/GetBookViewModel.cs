using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Book
{
    public class GetBookViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<GetBookDetails> BookDetails { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        public DesignInArgument<string> BookId { get; set; }

        #endregion

        public GetBookViewModel(IDesignServices services) : base(services)
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

            BookDetails.DisplayName = "BookDetails";
            BookDetails.Tooltip = "The book details get by id.";
            BookDetails.IsPrincipal = false;
            BookDetails.OrderIndex = 0;

            BookId.DisplayName = "BookDetails";
            BookId.Tooltip = "The book details get by id.";
            BookId.IsPrincipal = true;
            BookId.IsRequired = true;


        }
    }
}
