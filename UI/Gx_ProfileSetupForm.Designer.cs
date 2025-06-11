namespace GuardX.UI
{
    partial class Gx_ProfileSetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gx_ProfileSetupForm));
            logo_guardx = new PictureBox();
            txtBx_name = new TextBox();
            lb_name = new Label();
            lb_email = new Label();
            txtBx_email = new TextBox();
            label1 = new Label();
            txtBx_pwd = new TextBox();
            txtBx_uniqueText = new TextBox();
            lb_uniqueSentence = new Label();
            btn_save = new Button();
            btn_cancel = new Button();
            ((System.ComponentModel.ISupportInitialize)logo_guardx).BeginInit();
            SuspendLayout();
            // 
            // logo_guardx
            // 
            logo_guardx.Image = (Image)resources.GetObject("logo_guardx.Image");
            logo_guardx.InitialImage = (Image)resources.GetObject("logo_guardx.InitialImage");
            logo_guardx.Location = new Point(649, 12);
            logo_guardx.Name = "logo_guardx";
            logo_guardx.Size = new Size(139, 93);
            logo_guardx.TabIndex = 0;
            logo_guardx.TabStop = false;
            // 
            // txtBx_name
            // 
            txtBx_name.Location = new Point(22, 55);
            txtBx_name.Name = "txtBx_name";
            txtBx_name.Size = new Size(293, 31);
            txtBx_name.TabIndex = 1;
            // 
            // lb_name
            // 
            lb_name.AutoSize = true;
            lb_name.Location = new Point(22, 27);
            lb_name.Name = "lb_name";
            lb_name.Size = new Size(59, 25);
            lb_name.TabIndex = 2;
            lb_name.Text = "Name\r\n";
            // 
            // lb_email
            // 
            lb_email.AutoSize = true;
            lb_email.Location = new Point(24, 117);
            lb_email.Name = "lb_email";
            lb_email.Size = new Size(54, 25);
            lb_email.TabIndex = 3;
            lb_email.Text = "Email";
            // 
            // txtBx_email
            // 
            txtBx_email.Location = new Point(22, 155);
            txtBx_email.Name = "txtBx_email";
            txtBx_email.Size = new Size(293, 31);
            txtBx_email.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 221);
            label1.Name = "label1";
            label1.Size = new Size(87, 25);
            label1.TabIndex = 5;
            label1.Text = "Password";
            // 
            // txtBx_pwd
            // 
            txtBx_pwd.Location = new Point(24, 261);
            txtBx_pwd.Name = "txtBx_pwd";
            txtBx_pwd.Size = new Size(291, 31);
            txtBx_pwd.TabIndex = 6;
            // 
            // txtBx_uniqueText
            // 
            txtBx_uniqueText.Location = new Point(24, 355);
            txtBx_uniqueText.Name = "txtBx_uniqueText";
            txtBx_uniqueText.Size = new Size(473, 31);
            txtBx_uniqueText.TabIndex = 7;
            // 
            // lb_uniqueSentence
            // 
            lb_uniqueSentence.AutoSize = true;
            lb_uniqueSentence.Location = new Point(23, 327);
            lb_uniqueSentence.Name = "lb_uniqueSentence";
            lb_uniqueSentence.Size = new Size(237, 25);
            lb_uniqueSentence.TabIndex = 8;
            lb_uniqueSentence.Text = "Unique sentence/description";
            // 
            // btn_save
            // 
            btn_save.Location = new Point(676, 404);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(112, 34);
            btn_save.TabIndex = 9;
            btn_save.Text = "Save";
            btn_save.UseVisualStyleBackColor = true;
            btn_save.Click += btn_save_click;
            // 
            // btn_cancel
            // 
            btn_cancel.Location = new Point(558, 404);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(112, 34);
            btn_cancel.TabIndex = 10;
            btn_cancel.Text = "Cancel";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += btn_cancel_click;
            // 
            // Gx_ProfileSetupForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btn_cancel);
            Controls.Add(btn_save);
            Controls.Add(lb_uniqueSentence);
            Controls.Add(txtBx_uniqueText);
            Controls.Add(txtBx_pwd);
            Controls.Add(label1);
            Controls.Add(txtBx_email);
            Controls.Add(lb_email);
            Controls.Add(lb_name);
            Controls.Add(txtBx_name);
            Controls.Add(logo_guardx);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Gx_ProfileSetupForm";
            Text = "GuardX";
            ((System.ComponentModel.ISupportInitialize)logo_guardx).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox logo_guardx;
        private TextBox txtBx_name;
        private Label lb_name;
        private Label lb_email;
        private TextBox txtBx_email;
        private Label label1;
        private TextBox txtBx_pwd;
        private TextBox txtBx_uniqueText;
        private Label lb_uniqueSentence;
        private Button btn_save;
        private Button btn_cancel;
    }
}