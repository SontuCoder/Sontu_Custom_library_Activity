using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Book
{
    public class GetAllBookViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<int?> Limit { get; set; }
        public DesignInArgument<string> Cursor { get; set; }
        public DesignInArgument<string> BookType { get; set; }
        public DesignInArgument<string> BookName { get; set; }
        public DesignInArgument<string> BookAuthor { get; set; }
        public DesignInArgument<int?> BookEdition { get; set; }
        public DesignOutArgument<string> Error { get; set; }
        public DesignOutArgument<GetBooks> BookDetails { get; set; }

        #endregion

        public GetAllBookViewModel(IDesignServices services) : base(services)
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

            Limit.DisplayName = "Limit";
            Limit.Tooltip = "Limit of book list.";
            Limit.IsPrincipal = true;

            Cursor.DisplayName = "Cursor";
            Cursor.Tooltip = "First book id of the list.";
            Cursor.IsPrincipal = true;

            BookType.DisplayName = "BookTypes";
            BookType.Tooltip = "Pass the types of book. Like: mathematics, computer_science, physics, literature, others.";
            BookType.IsPrincipal = true;

            BookName.DisplayName = "BookName";
            BookName.Tooltip = "The Name of the perticular book";
            BookName.IsPrincipal = true;

            BookAuthor.DisplayName = "BookAuthor";
            BookAuthor.Tooltip = "The Name of the Auther.";
            BookAuthor.IsPrincipal = true;

            BookEdition.DisplayName = "BookEdition";
            BookEdition.Tooltip = "The edition of the book.";
            BookEdition.IsPrincipal = true;

        }
    }
}
