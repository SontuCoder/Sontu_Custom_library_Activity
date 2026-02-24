using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class AddNewBookViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<string> Title { get; set; }
        public DesignInArgument<string> Author { get; set; }
        public DesignInArgument<string> Description { get; set; }
        public DesignInArgument<string> Category { get; set; }
        public DesignInArgument<int> Edition { get; set; }
        public DesignInArgument<int> Quantity { get; set; }
        public DesignOutArgument<AddBookResponse> AddBookRes { get; set; }
        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public AddNewBookViewModel(IDesignServices services) : base(services)
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

            AddBookRes.DisplayName = "AddedNewBookResponse";
            AddBookRes.Tooltip = "The details of the new book.";
            AddBookRes.IsPrincipal = false;
            AddBookRes.OrderIndex = 1;

            Title.DisplayName = "Book Title";
            Title.Tooltip = "The book title.";
            Title.IsPrincipal = true;
            Title.IsRequired = true;
            Title.OrderIndex = 0;

            Author.DisplayName = "Book Author";
            Author.Tooltip = "The book author.";
            Author.IsPrincipal = true;
            Author.IsRequired = true;
            Author.OrderIndex = 1;

            Description.DisplayName = "Description";
            Description.Tooltip = "The book description.";
            Description.IsPrincipal = true;
            Description.IsRequired = true;
            Description.OrderIndex = 2;

            Category.DisplayName = "Categories";
            Category.Tooltip = "Write categories separeted by \",\". Ex: mathematics,physics,computer_science,literature,others";
            Category.IsPrincipal = true;
            Category.OrderIndex = 3;

            Edition.DisplayName = "Book Edition";
            Edition.Tooltip = "The book edition.";
            Edition.IsPrincipal = true;
            Edition.IsRequired = true;
            Edition.OrderIndex = 4;

            Quantity.DisplayName = "Quantity";
            Quantity.Tooltip = "The books quantity.";
            Quantity.IsPrincipal = true;
            Quantity.IsRequired = true;
            Quantity.OrderIndex = 5;
        }
    }
}
