using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class GetStudentDetailsByIdViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<string> StudentId { get; set; }
        public DesignOutArgument<StudentDetails> StudentDetails { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetStudentDetailsByIdViewModel(IDesignServices services) : base(services)
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

            StudentId.DisplayName = "StudentId";
            StudentId.Tooltip = "The unique student id.";
            StudentId.IsPrincipal = true;
            StudentId.IsRequired = true;
            StudentId.OrderIndex = 0;

            StudentDetails.DisplayName = "StudentDetails";
            StudentDetails.Tooltip = "The details of the student.";
            StudentDetails.IsPrincipal = false;
            StudentDetails.OrderIndex = 1;

        }
    }
}
