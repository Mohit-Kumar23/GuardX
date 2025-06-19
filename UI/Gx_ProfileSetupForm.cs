using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GuardX.UI
{
    public partial class Gx_ProfileSetupForm : Form
    {
        //Injection of services
        private readonly IServiceProvider _serviceProvider;
        private readonly IRegistryServices _registryServices;
        private readonly ILogger<Gx_ProfileSetupForm> _logger;

        private bool emailVerified = false;
        public Gx_ProfileSetupForm(IServiceProvider serviceProvider, IRegistryServices registryServices,ILogger<Gx_ProfileSetupForm> logger)
        {
            _serviceProvider = serviceProvider;
            _registryServices = registryServices;
            _logger = logger;

            InitializeComponent();
            FillProfileViews();
        }

        /// <summary>
        /// If General Profile is already registered then fill the Name and Email and then disable it for editing.
        /// </summary>
        private void FillProfileViews()
        {
            bool bResult = _registryServices.IsProfileCreated();
            if(bResult)
            {
                txtBx_name.Text = _registryServices.GetProfileName();
                txtBx_name.Enabled = false;
                txtBx_email.Text = _registryServices.GetProfileEmail();
                txtBx_email.Enabled = false;    
                btn_verify.Visible = false;
                emailVerified = true;
            }
        }

        /// <summary>
        /// Proceed to check the UI components value and update the registry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_click(object sender, EventArgs e)
        {
            //When all the fields are not empty then proceed to update the registry.
            if (!String.IsNullOrEmpty(txtBx_name.Text) && !String.IsNullOrEmpty(txtBx_email.Text)
                && !String.IsNullOrEmpty(txtBx_pwd.Text) && !String.IsNullOrEmpty(txtBx_uniqueText.Text))
            {
                errorProvider.Clear();

                if (emailVerified)
                {
                    EResult result = EResult.OK;

                    if (!_registryServices.IsProfileCreated())
                    {
                        result = _registryServices.CreateProfile(txtBx_name.Text, txtBx_email.Text);
                    }
                    if (result == EResult.OK)
                    {
                        //Encryption Alogrithm to take the password and unique string to store the plain text and cipher text
                        result = _registryServices.UpdateProfile(txtBx_pwd.Text, txtBx_uniqueText.Text);
                    }
                    if (result == EResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;                        
                    }
                    else
                    {
                        this.DialogResult= DialogResult.Cancel;
                    }
                    Close();
                }
                else
                {
                    errorProvider.SetError(txtBx_email, Constants.EMAIL_NOT_VERIFIED_ERROR);
                }
            }
            //Set the error provider for the UI components which are blank.
            else
            {
                if (String.IsNullOrEmpty(txtBx_name.Text))
                {
                    errorProvider.SetError(txtBx_name, Constants.NAME_FIELD_EMPTY_ERROR);
                }
                else
                {
                    errorProvider.SetError(txtBx_name,"");
                }
                if (String.IsNullOrEmpty(txtBx_email.Text))
                {
                    errorProvider.SetError(txtBx_email, Constants.EMAIL_FIELD_EMPTY_ERROR);
                }
                else
                {
                    errorProvider.SetError(txtBx_email, "");
                }
                if (String.IsNullOrEmpty(txtBx_pwd.Text))
                {
                    errorProvider.SetError(txtBx_pwd, Constants.PWD_FIELD_EMPTY_ERROR);
                }
                else
                {
                    errorProvider.SetError(txtBx_pwd, "");
                }
                if (String.IsNullOrEmpty(txtBx_uniqueText.Text))
                {
                    errorProvider.SetError(txtBx_uniqueText, Constants.SENTENCE_FIELD_EMPTY_ERROR);
                }
                else
                {
                    errorProvider.SetError(txtBx_uniqueText, "");
                }
            }
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Open the Otp Form if email is verified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_verify_click(object sender,EventArgs e)
        {
            //If email is not blank then proceed to open the OTP form.
            if(!String.IsNullOrEmpty(txtBx_email.Text) && !String.IsNullOrEmpty(txtBx_name.Text))
            {
                errorProvider.SetError(txtBx_email, "");
                EResult eResult = EResult.OK;

                //Open the Gx_OtpForm
                using (var otpFrom = _serviceProvider.GetRequiredService<Gx_OtpFrom>())
                {
                    otpFrom.init(txtBx_email.Text);
                    eResult = otpFrom.GenerateOtpAndSendEmail(txtBx_name.Text, txtBx_email.Text, EEmailPurpose.SetupProfile);

                    otpFrom.ShowDialog();

                    if (otpFrom.DialogResult == DialogResult.OK)
                    {
                        eResult = EResult.OK;
                    }
                    else
                    {
                        eResult = EResult.ERROR;
                    }
                }
                //If email is verified then change button color and disable it.
                if (EResult.OK == eResult)
                {
                    btn_verify.Text = "Verified!";
                    btn_verify.BackColor = Color.LightGreen;
                    btn_verify.Enabled = false;
                    emailVerified = true;
                }
            }
            //Set error provider if email or name is blank.
            else
            {
                if(String.IsNullOrEmpty(txtBx_email.Text))
                    errorProvider.SetError(txtBx_email,Constants.EMAIL_FIELD_EMPTY_ERROR);
                else
                    errorProvider.SetError(txtBx_email,"");

                if(String.IsNullOrEmpty(txtBx_name.Text))
                    errorProvider.SetError(txtBx_name,Constants.NAME_FIELD_EMPTY_ERROR);
                else
                    errorProvider.SetError(txtBx_name, "");
            }           
        }

        /// <summary>
        /// Fill and Disable the Name, Email and Unique Sentence UI Components for Forgot password process.
        /// </summary>
        public void SetUpForResetPassword()
        {
            this.txtBx_name.Text = _registryServices.GetProfileName();
            this.txtBx_name.Enabled = false;

            this.txtBx_email.Text = _registryServices.GetProfileEmail();
            this.txtBx_email.Enabled = false;

            this.txtBx_uniqueText.Text = _registryServices.GetUniqueSentence();
            this.txtBx_uniqueText.Enabled = false;

            this.btn_verify.Visible = false;
            this.emailVerified = true;
        }
    }
}
