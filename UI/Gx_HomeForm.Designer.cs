namespace GuardX
{
    partial class Gx_HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gx_HomeForm));
            btn_hide = new Button();
            btn_unhide = new Button();
            panel_separator = new Panel();
            btn_profileSetup = new Button();
            btn_deleteProfile = new Button();
            btn_forgotPassword = new Button();
            logo_guardx = new PictureBox();
            label = new Label();
            ((System.ComponentModel.ISupportInitialize)logo_guardx).BeginInit();
            SuspendLayout();
            // 
            // btn_hide
            // 
            btn_hide.Font = new Font("Segoe UI", 10F);
            btn_hide.Image = (Image)resources.GetObject("btn_hide.Image");
            btn_hide.Location = new Point(22, 28);
            btn_hide.Name = "btn_hide";
            btn_hide.Size = new Size(173, 185);
            btn_hide.TabIndex = 0;
            btn_hide.Text = "Hide";
            btn_hide.TextAlign = ContentAlignment.BottomCenter;
            btn_hide.TextImageRelation = TextImageRelation.ImageAboveText;
            btn_hide.UseVisualStyleBackColor = true;
            // 
            // btn_unhide
            // 
            btn_unhide.Font = new Font("Segoe UI", 10F);
            btn_unhide.Image = (Image)resources.GetObject("btn_unhide.Image");
            btn_unhide.Location = new Point(229, 28);
            btn_unhide.Name = "btn_unhide";
            btn_unhide.Size = new Size(177, 185);
            btn_unhide.TabIndex = 1;
            btn_unhide.Text = "Unhide";
            btn_unhide.TextAlign = ContentAlignment.BottomCenter;
            btn_unhide.TextImageRelation = TextImageRelation.ImageAboveText;
            btn_unhide.UseVisualStyleBackColor = true;
            // 
            // panel_separator
            // 
            panel_separator.BackColor = Color.Black;
            panel_separator.Location = new Point(9, 234);
            panel_separator.Name = "panel_separator";
            panel_separator.Size = new Size(773, 1);
            panel_separator.TabIndex = 2;
            // 
            // btn_profileSetup
            // 
            btn_profileSetup.Font = new Font("Segoe UI", 10F);
            btn_profileSetup.Image = (Image)resources.GetObject("btn_profileSetup.Image");
            btn_profileSetup.Location = new Point(22, 253);
            btn_profileSetup.Name = "btn_profileSetup";
            btn_profileSetup.Size = new Size(173, 185);
            btn_profileSetup.TabIndex = 3;
            btn_profileSetup.Text = "Setup Profile";
            btn_profileSetup.TextAlign = ContentAlignment.BottomCenter;
            btn_profileSetup.TextImageRelation = TextImageRelation.ImageAboveText;
            btn_profileSetup.UseVisualStyleBackColor = true;
            btn_profileSetup.Click += btn_profileSetup_Click;
            // 
            // btn_deleteProfile
            // 
            btn_deleteProfile.Font = new Font("Segoe UI", 10F);
            btn_deleteProfile.Image = (Image)resources.GetObject("btn_deleteProfile.Image");
            btn_deleteProfile.Location = new Point(229, 253);
            btn_deleteProfile.Name = "btn_deleteProfile";
            btn_deleteProfile.Size = new Size(173, 185);
            btn_deleteProfile.TabIndex = 4;
            btn_deleteProfile.Text = "Delete Profile";
            btn_deleteProfile.TextAlign = ContentAlignment.BottomCenter;
            btn_deleteProfile.TextImageRelation = TextImageRelation.ImageAboveText;
            btn_deleteProfile.UseVisualStyleBackColor = true;
            // 
            // btn_forgotPassword
            // 
            btn_forgotPassword.Font = new Font("Segoe UI", 10F);
            btn_forgotPassword.Image = (Image)resources.GetObject("btn_forgotPassword.Image");
            btn_forgotPassword.Location = new Point(437, 253);
            btn_forgotPassword.Name = "btn_forgotPassword";
            btn_forgotPassword.Size = new Size(173, 185);
            btn_forgotPassword.TabIndex = 5;
            btn_forgotPassword.Text = "Forgot Password";
            btn_forgotPassword.TextAlign = ContentAlignment.BottomCenter;
            btn_forgotPassword.TextImageRelation = TextImageRelation.ImageAboveText;
            btn_forgotPassword.UseVisualStyleBackColor = true;
            // 
            // logo_guardx
            // 
            logo_guardx.Image = (Image)resources.GetObject("logo_guardx.Image");
            logo_guardx.InitialImage = (Image)resources.GetObject("logo_guardx.InitialImage");
            logo_guardx.Location = new Point(643, 12);
            logo_guardx.Name = "logo_guardx";
            logo_guardx.Size = new Size(139, 93);
            logo_guardx.TabIndex = 6;
            logo_guardx.TabStop = false;
            // 
            // label
            // 
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 6F);
            label.Location = new Point(643, 108);
            label.Name = "label";
            label.Size = new Size(144, 30);
            label.TabIndex = 7;
            label.Text = "Design and Developed by \nMohit Kumar";
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Gx_HomeForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label);
            Controls.Add(logo_guardx);
            Controls.Add(btn_forgotPassword);
            Controls.Add(btn_deleteProfile);
            Controls.Add(btn_profileSetup);
            Controls.Add(panel_separator);
            Controls.Add(btn_unhide);
            Controls.Add(btn_hide);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Gx_HomeForm";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "GuardX";
            ((System.ComponentModel.ISupportInitialize)logo_guardx).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_hide;
        private Button btn_unhide;
        private Panel panel_separator;
        private Button btn_profileSetup;
        private Button btn_deleteProfile;
        private Button btn_forgotPassword;
        private PictureBox logo_guardx;
        private Label label;
    }
}
