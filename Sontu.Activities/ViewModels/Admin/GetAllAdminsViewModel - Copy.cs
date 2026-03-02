using Sontu.Activities.Models;
using System.Activities.DesignViewModels;

namespace Sontu.Activities.ViewModels.Admin
{
    public class GetAllAdminsViewModel : DesignPropertiesViewModel
    {

        #region Design Properties

        public DesignOutArgument<AllAdmins> ListOfAdmins { get; set; }

        public DesignOutArgument<string> Error { get; set; }

        #endregion

        public GetAllAdminsViewModel(IDesignServices services) : base(services)
        {
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            Error.DisplayName = "Error";
            Error.Tooltip = "The error message.";
            Error.IsPrincipal = false;

            ListOfAdmins.DisplayName = "ListOfAllAdmins";
            ListOfAdmins.Tooltip = "The list of all admins details.";
            ListOfAdmins.IsPrincipal = true;
            ListOfAdmins.OrderIndex = 0;

        }
    }
}
