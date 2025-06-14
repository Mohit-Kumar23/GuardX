namespace GuardX.UI
{
    partial class Gx_OtpFrom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gx_OtpFrom));
            lblForEmail = new Label();
            btn_verify = new Button();
            mskTxtOtp = new MaskedTextBox();
            btn_show_hide_otp = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblForEmail
            // 
            lblForEmail.Location = new Point(22, 47);
            lblForEmail.Name = "lblForEmail";
            lblForEmail.Size = new Size(452, 51);
            lblForEmail.TabIndex = 0;
            lblForEmail.Text = "Enter OTP sent to email:";
            lblForEmail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_verify
            // 
            btn_verify.Location = new Point(249, 149);
            btn_verify.Name = "btn_verify";
            btn_verify.Size = new Size(112, 34);
            btn_verify.TabIndex = 1;
            btn_verify.Text = "Verify";
            btn_verify.UseVisualStyleBackColor = true;
            btn_verify.Click += btn_verify_Click;
            // 
            // mskTxtOtp
            // 
            mskTxtOtp.CutCopyMaskFormat = MaskFormat.IncludePromptAndLiterals;
            mskTxtOtp.InsertKeyMode = InsertKeyMode.Overwrite;
            mskTxtOtp.Location = new Point(120, 101);
            mskTxtOtp.Mask = "000000";
            mskTxtOtp.Name = "mskTxtOtp";
            mskTxtOtp.PromptChar = '*';
            mskTxtOtp.Size = new Size(241, 31);
            mskTxtOtp.TabIndex = 2;
            mskTxtOtp.TextAlign = HorizontalAlignment.Center;
            mskTxtOtp.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            mskTxtOtp.UseSystemPasswordChar = true;
            // 
            // btn_show_hide_otp
            // 
            btn_show_hide_otp.Location = new Point(131, 149);
            btn_show_hide_otp.Name = "btn_show_hide_otp";
            btn_show_hide_otp.Size = new Size(112, 34);
            btn_show_hide_otp.TabIndex = 3;
            btn_show_hide_otp.Text = "Show/Hide";
            btn_show_hide_otp.UseVisualStyleBackColor = true;
            btn_show_hide_otp.Click += btn_show_hide_otp_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // Gx_OtpFrom
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 243);
            Controls.Add(btn_show_hide_otp);
            Controls.Add(mskTxtOtp);
            Controls.Add(btn_verify);
            Controls.Add(lblForEmail);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Gx_OtpFrom";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "OTP Validation";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblForEmail;
        private Button btn_verify;
        private MaskedTextBox mskTxtOtp;
        private Button btn_show_hide_otp;
        private ErrorProvider errorProvider;
    }
}