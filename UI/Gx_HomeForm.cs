using GuardX.BLServices;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using GuardX.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GuardX
{
    public partial class Gx_HomeForm : Form
    {
        //Enabling/Disabling Flags for UI Components
        private EEnableAction enableActionsFlag = EEnableAction.DisableAll;
        
        //Injected Services
        private readonly IIdentificationFileService _identificationFileService;
        private readonly IRegistryServices _registryServices;
        private readonly IServiceProvider _serviceProvider;
        private readonly IVisibilityService _visibilityService;
        private readonly ILogger<Gx_HomeForm> _logger;
        public Gx_HomeForm(IServiceProvider serviceProvider, IIdentificationFileService identificationService, IRegistryServices registryServices, IVisibilityService visibilityService, ILogger<Gx_HomeForm> logger)
        {
            _serviceProvider = serviceProvider;
            _identificationFileService = identificationService;
            _registryServices = registryServices;
            _visibilityService = visibilityService;
            _logger = logger;

            InitializeComponent();
            init();
        }

        private void init()
        {
            //Check if IDN file is present and in correct format
            bool bIDNCorrect = _identificationFileService.IsIDNFilePresentOrFormatted();

            if (bIDNCorrect)
            {
                //Proceed to check the registry if IDN file is present and in correct format
                EResult eResult = CheckRegistry();


                if (eResult == EResult.ERROR)
                {
                    //Close application if registry is not in correct format.
                    Environment.Exit(0);
                }
            }
            EnableDisableUIControls();
        }

        /// <summary>
        /// To Check the status of application profile in the registry
        /// </summary>
        /// <returns>
        /// EResult
        /// </returns>
        private EResult CheckRegistry()
        {
            EResult eResult = EResult.OK;

            ERegistryResults registryResults = _registryServices.CheckApplicationRegistryAndUniqueness();

            //If Non-unique name then show message box
            if (registryResults == ERegistryResults.NonUniqueName)
            {
                DialogResult result = MessageBox.Show(String.Format(Constants.INVALID_UNIQUE_NAME, Constants.IDN_FILE_NAME), Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                eResult = EResult.ERROR;
            }
            //If registration is required then register the application with basic information
            else if (registryResults == ERegistryResults.RegRequired)
            {
                EResult result = _registryServices.RegisterApplication();

                if (result == EResult.ERROR)
                {
                    DialogResult dialogResult = MessageBox.Show(Constants.APPLICATION_ERROR, Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    eResult = EResult.ERROR;
                }
                else
                {
                    //Disable All buttons except the "Create Profile" Button
                    UpdateEnableActionFlag(EEnableAction.ProfileSetup, true);
                    UpdateEnableActionFlag(EEnableAction.HideAction, false);
                    UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
                    UpdateEnableActionFlag(EEnableAction.DeleteProfile, false);
                    UpdateEnableActionFlag(EEnableAction.ForgotPwd, false);
                }

            }
            //If the format is not correct then throw error.
            else if (registryResults == ERegistryResults.InvalidFormat)
            {
                DialogResult dialogResult = MessageBox.Show(Constants.APPLICATION_ERROR, Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                eResult = EResult.ERROR;
            }
            //If update in registry is required then show the message box and Enable/Disable the necessary UI components.
            else if (registryResults == ERegistryResults.UpdateRequired)
            {
                DialogResult dialogResult = MessageBox.Show(Constants.UPDATE_REGX_REQ, Constants.PROFILE_SETUP_REQ_TITLE, MessageBoxButtons.OK);

                //Disable All buttons except the "Create Profile" Button
                UpdateEnableActionFlag(EEnableAction.ProfileSetup, true);
                UpdateEnableActionFlag(EEnableAction.HideAction, false);
                UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
                UpdateEnableActionFlag(EEnableAction.DeleteProfile, false);
                UpdateEnableActionFlag(EEnableAction.ForgotPwd, false);
            }

            //If already registered, then enable/disable the necessary UI components.
            if (registryResults == ERegistryResults.AlreadyReg)
            {
                //Check for Files Hidden or Not, based on that Disable the buttons. Also Disable ProfileSetup
                UpdateEnableActionFlag(EEnableAction.ProfileSetup, false);
                UpdateEnableActionFlag(EEnableAction.DeleteProfile, true);
                UpdateEnableActionFlag(EEnableAction.ForgotPwd, true);
                SetHiddenOrUnhiddenFlagAsPerFileVisibility();
            }

            return eResult;
        }

        /// <summary>
        /// Check if the Files except the GuardX.exe, guardx_congif.idn and guardX_log files are hidden or visible and enable the UI components accordingly.
        /// </summary>
        public void SetHiddenOrUnhiddenFlagAsPerFileVisibility()
        {
            String currentDirectory = Directory.GetCurrentDirectory();

            bool bFilesHidden = true;

            foreach (var entity in Directory.EnumerateFileSystemEntries(currentDirectory))
            {
                var fileName = Path.GetFileName(entity);
                if (!fileName.Equals(Constants.APP_NAME_EXE) && !fileName.Equals(Constants.IDN_FILE_NAME)
                    && !fileName.Contains(Constants.GUARDX_LOG_SUBSTRING))
                {
                    if ((File.GetAttributes(entity) & FileAttributes.Hidden) == 0)
                    {
                        bFilesHidden = false;
                        break;
                    }
                }
            }

            if (bFilesHidden)
            {
                UpdateEnableActionFlag(EEnableAction.HideAction, false);
                UpdateEnableActionFlag(EEnableAction.UnHideAction, true);
            }
            else
            {
                UpdateEnableActionFlag(EEnableAction.HideAction, true);
                UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
            }

        }

        /// <summary>
        /// Update the UI Flag
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="bCalledToEnable"></param>
        private void UpdateEnableActionFlag(EEnableAction flag, bool bCalledToEnable)
        {
            if (bCalledToEnable)
            {
                enableActionsFlag |= flag;
            }
            else
            {
                enableActionsFlag &= ~flag;
            }
        }

        /// <summary>
        /// Enable/Disable the UI Components based on Flag
        /// </summary>
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
                btn_hide.Enabled = false;
                btn_unhide.Enabled = false;
                btn_profileSetup.Enabled = false;
                btn_deleteProfile.Enabled = false;
                btn_forgotPassword.Enabled = false;
            }
        }

        /// <summary>
        /// Opens the Profile Setup form on ProfileSetup Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_profileSetup_Click(object sender, EventArgs e)
        {
            using (var gx_ProfileSetupForm = _serviceProvider.GetRequiredService<Gx_ProfileSetupForm>())
            {
                gx_ProfileSetupForm.ShowDialog();

                if (gx_ProfileSetupForm.DialogResult == DialogResult.OK)
                {
                    //Enable only Hide, Delete Profile and Forgot Password button
                    UpdateEnableActionFlag(EEnableAction.ProfileSetup, false);
                    UpdateEnableActionFlag(EEnableAction.HideAction, true);
                    UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
                    UpdateEnableActionFlag(EEnableAction.DeleteProfile, true);
                    UpdateEnableActionFlag(EEnableAction.ForgotPwd, true);
                    EnableDisableUIControls();
                }
            }
        }

        /// <summary>
        /// Verify Password and Hides the files and directories and update the UI flags accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_hide_Click(object sender, EventArgs e)
        {
            using (var gx_PasswordInputForm = _serviceProvider.GetRequiredService<Gx_PasswordInputForm>())
            {
                gx_PasswordInputForm.ShowDialog();

                if (gx_PasswordInputForm.DialogResult == DialogResult.OK)
                {
                    EResult eResult = _visibilityService.Hide();

                    if (EResult.OK == eResult)
                    {
                        UpdateEnableActionFlag(EEnableAction.HideAction, false);
                        UpdateEnableActionFlag(EEnableAction.UnHideAction, true);
                        EnableDisableUIControls();
                    }
                }
            }
        }

        /// <summary>
        /// Verify Password and UnHides the files and directories and update the UI flags accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_unhide_Click(object sender, EventArgs e)
        {
            using (var gx_PasswordInputForm = _serviceProvider.GetRequiredService<Gx_PasswordInputForm>())
            {
                gx_PasswordInputForm.ShowDialog();

                if (gx_PasswordInputForm.DialogResult == DialogResult.OK)
                {
                    EResult eResult = _visibilityService.UnHide();
                    if (EResult.OK == eResult)
                    {
                        UpdateEnableActionFlag(EEnableAction.HideAction, true);
                        UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
                        EnableDisableUIControls();
                    }
                }
            }
        }

        /// <summary>
        /// Delete the profiles
        /// Verify the Password->Unhide all the files->Delete the Profile from Registry->Delete the IDN File.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteProfile_Click(object sender, EventArgs e)
        {
            using (var gx_PasswordInputForm = _serviceProvider.GetRequiredService<Gx_PasswordInputForm>())
            {
                gx_PasswordInputForm.ShowDialog();

                if (gx_PasswordInputForm.DialogResult == DialogResult.OK)
                {
                    EResult eResult = _visibilityService.UnHide();
                    if (EResult.OK == eResult)
                    {
                        _registryServices.DeleteRegistryProfile();
                        if (EResult.OK == eResult)
                        {
                            eResult = _identificationFileService.DeleteIDNFile();
                            if (EResult.OK == eResult)
                            {
                                Thread.Sleep(1500);
                                Environment.Exit(0);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens the OTP form to reset the profile.
        /// Verify the OTP, Unhide the files and Open the Profile setup form for password reset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_forgotPassword_Click(object sender, EventArgs e)
        {
            using(var otpForm = _serviceProvider.GetRequiredService<Gx_OtpFrom>())
            {
                EResult eResult;
                otpForm.init(_registryServices.GetProfileEmail());
                
                //Send OTP Email
                eResult = otpForm.GenerateOtpAndSendEmail(_registryServices.GetProfileName(),_registryServices.GetProfileEmail(),EEmailPurpose.ResetProfile);
                
                otpForm.ShowDialog();

                if (otpForm.DialogResult == DialogResult.OK)
                { 
                    if (eResult == EResult.OK)
                    {
                        //Unhide the Files and Directories
                        eResult = _visibilityService.UnHide();
                        if (eResult == EResult.OK)
                        {
                            UpdateEnableActionFlag(EEnableAction.HideAction, true);
                            UpdateEnableActionFlag(EEnableAction.UnHideAction, false);
                            EnableDisableUIControls();

                            using (var profileSetUpForm = _serviceProvider.GetRequiredService<Gx_ProfileSetupForm>())
                            {
                                //Open Profile Form to reset password.
                                profileSetUpForm.SetUpForResetPassword();
                                profileSetUpForm.ShowDialog();
                            }
                        }
                    }
                }
            }
        }
    }
}
