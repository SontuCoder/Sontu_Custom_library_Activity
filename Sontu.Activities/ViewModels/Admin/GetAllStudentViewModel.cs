using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class RequestBookViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<ListOfStudents> ListOfStudents { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public RequestBookViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;

            ListOfStudents.DisplayName = "ListOfStudents";
            ListOfStudents.Tooltip = "The list of all students details.";
            ListOfStudents.IsPrincipal = true;
            ListOfStudents.OrderIndex = 0;

        }
    }
}
