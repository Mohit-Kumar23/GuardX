using GuardX.BLServices;
using GuardX.Common;
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
                EResult eResult = CheckRegistry();

                if(eResult == EResult.ERROR)
                {
                    Environment.Exit(0);
                }
                else
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

        private EResult CheckRegistry()
        {
            EResult eResult = EResult.OK;

            ERegistryResults registryResults = _registryServices.CheckApplicationRegistryAndUniqueness();

            if (registryResults == ERegistryResults.NonUniqueName)
            {
                DialogResult result = MessageBox.Show(String.Format(Constants.INVALID_UNIQUE_NAME, Constants.IDN_FILE_NAME), Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                eResult = EResult.ERROR;
            }
            else if (registryResults == ERegistryResults.RegRequired)
            {
                EResult result = _registryServices.RegisterApplication();
                if (result == EResult.ERROR)
                {
                    DialogResult dialogResult = MessageBox.Show(Constants.APPLICATION_ERROR, Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    eResult = EResult.ERROR;
                }
            }
            else if (registryResults == ERegistryResults.InvalidFormat)
            {
                DialogResult dialogResult = MessageBox.Show(Constants.APPLICATION_ERROR, Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                eResult = EResult.ERROR;
            }

            return eResult;
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
                if ((enableActionsFlag & EEnableAction.HideAction) == EEnableAction.HideAction)
                {
                    btn_hide.Enabled = true;
                }
                else
                {
                    btn_hide.Enabled = false;
                }
                if ((enableActionsFlag & EEnableAction.UnHideAction) == EEnableAction.UnHideAction)
                {
                    btn_unhide.Enabled = true;
                }
                else
                {
                    btn_unhide.Enabled = false;
                }
                if ((enableActionsFlag & EEnableAction.ProfileSetup) == EEnableAction.ProfileSetup)
                {
                    btn_profileSetup.Enabled = true;
                }
                else
                {
                    btn_profileSetup.Enabled = false;
                }
                if ((enableActionsFlag & EEnableAction.DeleteProfile) == EEnableAction.DeleteProfile)
                {
                    btn_deleteProfile.Enabled = true;
                }
                else
                {
                    btn_deleteProfile.Enabled = false;
                }
                if ((enableActionsFlag & EEnableAction.ForgotPwd) == EEnableAction.ForgotPwd)
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
