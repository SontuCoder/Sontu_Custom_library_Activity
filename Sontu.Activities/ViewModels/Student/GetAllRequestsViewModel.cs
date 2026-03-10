using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Student
{
    public class GetAllRequestsViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<ListOfStudentRequests> AllRequests { get; set; }
        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetAllRequestsViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;
            Error.OrderIndex = 0;

            AllRequests.DisplayName = "AllRequests";
            AllRequests.Tooltip = "Api response for getting all requests of current student.";
            AllRequests.IsPrincipal = true;
            AllRequests.OrderIndex = 0;


        }
    }
}
