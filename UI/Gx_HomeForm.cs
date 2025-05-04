using GuardX.BLServices;
using GuardX.Enums;
using GuardX.Interfaces;

namespace GuardX
{
    public partial class Gx_HomeForm : Form
    {
        private EEnableAction enableActionsFlag = EEnableAction.DisableAll;
        private readonly IIdentificationFileService _identificationFileService;
        private readonly IRegistryServices _registryServices;
        public Gx_HomeForm(IIdentificationFileService identificationService,IRegistryServices registryServices)
        {        
            _identificationFileService = identificationService;
            _registryServices = registryServices;
            InitializeComponent();
            init();           
        }

        private void init()
        {
            bool bIDNCorrect = _identificationFileService.IsIDNFilePresentOrFormatted();
            if (bIDNCorrect)
            {
                bool bIdentifierUnique = false;// registryServices.IsAppIdentifierUniqueInRegistry();
                if (bIdentifierUnique)
                {
                    UpdateEnableActionFlag(EEnableAction.HideAction, true);
                    UpdateEnableActionFlag(EEnableAction.UnHideAction, true);
                    UpdateEnableActionFlag(EEnableAction.ProfileSetup, true);
                    UpdateEnableActionFlag(EEnableAction.DeleteProfile, true);
                    UpdateEnableActionFlag(EEnableAction.ForgotPwd, true);
                }
            }
            EnableDisableUIControls();
        }

        private void UpdateEnableActionFlag(EEnableAction flag,bool bCalledToEnable)
        {
            if(bCalledToEnable)
            {
                enableActionsFlag |= flag;
            }
            else
            {
                enableActionsFlag &= ~flag;
            }
        }
        private void EnableDisableUIControls()
        {
            if (enableActionsFlag != EEnableAction.DisableAll)
            {
                if (enableActionsFlag == EEnableAction.HideAction)
                {
                    btn_hide.Enabled = true;
                }
                else
                {
                    btn_hide.Enabled = false;
                }
                if (enableActionsFlag == EEnableAction.UnHideAction)
                {
                    btn_unhide.Enabled = true;
                }
                else
                {
                    btn_unhide.Enabled = false;
                }
                if (enableActionsFlag == EEnableAction.ProfileSetup)
                {
                    btn_profileSetup.Enabled = true;
                }
                else
                {
                    btn_profileSetup.Enabled = false;
                }
                if (enableActionsFlag == EEnableAction.DeleteProfile)
                {
                    btn_deleteProfile.Enabled = true;
                }
                else
                {
                    btn_deleteProfile.Enabled = false;
                }
                if (enableActionsFlag == EEnableAction.ForgotPwd)
                {
                    btn_forgotPassword.Enabled = true;
                }
                else
                {
                    btn_forgotPassword.Enabled = false;
                }
            }
            else
            {
                btn_hide.Enabled= false;
                btn_unhide.Enabled= false;
                btn_profileSetup.Enabled= false;
                btn_deleteProfile.Enabled= false;
                btn_forgotPassword.Enabled= false;
            }
        }
    }
}
