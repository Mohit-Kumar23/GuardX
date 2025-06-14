namespace GuardX.UI
{
    partial class Gx_PasswordInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gx_PasswordInputForm));
            lbl_enterPassword = new Label();
            btn_submit = new Button();
            txt_password = new TextBox();
            btn_show_hidePassword = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lbl_enterPassword
            // 
            lbl_enterPassword.AutoSize = true;
            lbl_enterPassword.Location = new Point(154, 40);
            lbl_enterPassword.Name = "lbl_enterPassword";
            lbl_enterPassword.Size = new Size(175, 25);
            lbl_enterPassword.TabIndex = 0;
            lbl_enterPassword.Text = "Enter your password";
            // 
            // btn_submit
            // 
            btn_submit.Location = new Point(253, 147);
            btn_submit.Name = "btn_submit";
            btn_submit.Size = new Size(112, 34);
            btn_submit.TabIndex = 1;
            btn_submit.Text = "Submit";
            btn_submit.UseVisualStyleBackColor = true;
            btn_submit.Click += btn_submit_Click;
            // 
            // txt_password
            // 
            txt_password.Location = new Point(135, 89);
            txt_password.Name = "txt_password";
            txt_password.ShortcutsEnabled = false;
            txt_password.Size = new Size(213, 31);
            txt_password.TabIndex = 2;
            txt_password.TextAlign = HorizontalAlignment.Center;
            txt_password.UseSystemPasswordChar = true;
            // 
            // btn_show_hidePassword
            // 
            btn_show_hidePassword.Location = new Point(135, 147);
            btn_show_hidePassword.Name = "btn_show_hidePassword";
            btn_show_hidePassword.Size = new Size(112, 34);
            btn_show_hidePassword.TabIndex = 3;
            btn_show_hidePassword.Text = "Show/Hide";
            btn_show_hidePassword.UseVisualStyleBackColor = true;
            btn_show_hidePassword.Click += btn_show_hidePassword_Click;
            // 
            // errorProvider
            // 
            errorProvider.ContainerControl = this;
            // 
            // Gx_PasswordInputForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 243);
            Controls.Add(btn_show_hidePassword);
            Controls.Add(txt_password);
            Controls.Add(btn_submit);
            Controls.Add(lbl_enterPassword);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Gx_PasswordInputForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Password Protected";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_enterPassword;
        private Button btn_submit;
        private TextBox txt_password;
        private Button btn_show_hidePassword;
        private ErrorProvider errorProvider;
    }
}