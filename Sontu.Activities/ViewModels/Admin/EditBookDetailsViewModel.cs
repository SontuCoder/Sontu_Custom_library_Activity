using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class EditBookDetailsViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<string> BookId { get; set; }
        public DesignInArgument<string> Title { get; set; }
        public DesignInArgument<string> Author { get; set; }
        public DesignInArgument<string> Description { get; set; }
        public DesignInArgument<string> Categories { get; set; }
        public DesignInArgument<int?> Edition { get; set; }
        public DesignOutArgument<APIResponse> EditBookResponse { get; set; }
        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public EditBookDetailsViewModel(IDesignServices services) : base(services)
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

            EditBookResponse.DisplayName = "EditBookResponse";
            EditBookResponse.Tooltip = "The response of the edit book.";
            EditBookResponse.IsPrincipal = false;
            EditBookResponse.OrderIndex = 1;

            BookId.DisplayName = "Book Id";
            BookId.Tooltip = "The book Id.";
            BookId.IsPrincipal = true;
            BookId.IsRequired = true;
            BookId.OrderIndex = 0;

            Title.DisplayName = "Book Title";
            Title.Tooltip = "The book title.";
            Title.IsPrincipal = true;
            Title.OrderIndex = 1;

            Author.DisplayName = "Book Author";
            Author.Tooltip = "The book author.";
            Author.IsPrincipal = true;
            Author.OrderIndex = 2;

            Description.DisplayName = "Description";
            Description.Tooltip = "The book description.";
            Description.IsPrincipal = true;
            Description.OrderIndex = 3;

            Categories.DisplayName = "Categories";
            Categories.Tooltip = "Write categories separeted by \",\". Ex: mathematics,physics,computer_science,literature,others";
            Categories.IsPrincipal = true;
            Categories.OrderIndex = 4;

            Edition.DisplayName = "Book Edition";
            Edition.Tooltip = "The book edition.";
            Edition.IsPrincipal = true;
            Edition.OrderIndex = 5;
        }
    }
}
