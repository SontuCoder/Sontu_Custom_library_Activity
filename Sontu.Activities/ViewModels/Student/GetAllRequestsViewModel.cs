using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Student
{
    public class GetAllRequestsViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignInArgument<StudentRequestStatus> RequestStatus { get; set; }
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
            Error.OrderIndex = 1;

            AllRequests.DisplayName = "AllRequests";
            AllRequests.Tooltip = "Api response for getting all requests of current student.";
            AllRequests.IsPrincipal = false;
            AllRequests.OrderIndex = 0;

            RequestStatus.DisplayName = "RequestStatus";
            RequestStatus.Tooltip = "The status of the requests to retrieve. Valid values are 'All' or 'Issued'.";
            RequestStatus.IsPrincipal = true;
            RequestStatus.OrderIndex = 1;
            RequestStatus.IsRequired = true;


        }
    }
}
