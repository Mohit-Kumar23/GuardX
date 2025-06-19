using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.Extensions.Logging;

namespace GuardX.UI
{
    public partial class Gx_OtpFrom : Form
    {
        //Injection of Services
        private readonly IEmailService _emailService;
        private readonly ILogger<Gx_OtpFrom> _logger;
        private int otpValue = 0;

        public Gx_OtpFrom(IEmailService emailService,ILogger<Gx_OtpFrom> logger)
        {
            _emailService = emailService;
            _logger = logger;

            this.DialogResult = DialogResult.Cancel;
            InitializeComponent();
        }

        public void init(string email)
        {
            lblForEmail.Text = lblForEmail.Text + email;
        }

        /// <summary>
        /// Generate a random number of 6 digit and send it for OTP.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="eEmailPurpose"></param>
        /// <returns>
        /// EResult
        /// </returns>
        public EResult GenerateOtpAndSendEmail(String userName, String email,EEmailPurpose eEmailPurpose)
        {
            EResult result = EResult.OK;

            Random random = new Random();
            otpValue = random.Next(100000, 1000000);

            //Send Email for OTP verification
            result = _emailService.SendOtpEmail(userName,email,otpValue, eEmailPurpose);

            return result;
        }

        /// <summary>
        /// Show/Hide the OTP value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_show_hide_otp_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(mskTxtOtp.Text))
            {
                if (mskTxtOtp.UseSystemPasswordChar)
                {
                    mskTxtOtp.UseSystemPasswordChar = false;
                }
                else
                {
                    mskTxtOtp.UseSystemPasswordChar = true;
                }
            }
        }

        /// <summary>
        /// Check if the entered OTP is same as OTP generated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_verify_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(mskTxtOtp.Text))
            {
                if (mskTxtOtp.Text.Equals(otpValue.ToString()))
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    errorProvider.SetError(mskTxtOtp,Constants.INVALID_OTP);
                }
            }
            else
            {
                errorProvider.SetError(mskTxtOtp, Constants.OTP_FIELD_EMPTY_ERROR);
            }
        }
    }
}
