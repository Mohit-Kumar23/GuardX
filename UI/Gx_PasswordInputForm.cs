using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;

namespace GuardX.UI
{
    public partial class Gx_PasswordInputForm : Form
    {
        private readonly IRegistryServices _registryServices;
        public Gx_PasswordInputForm(IRegistryServices registryServices)
        {
            _registryServices = registryServices;
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_show_hidePassword_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_password.Text)) 
            { 
                if (txt_password.UseSystemPasswordChar)
                {
                    txt_password.UseSystemPasswordChar = false;
                }
                else
                {
                    txt_password.UseSystemPasswordChar = true;
                }
            }
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txt_password.Text))
            {
                EResult eResult;
                eResult = _registryServices.ValidatePassword(txt_password.Text);
                if(EResult.OK == eResult)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    errorProvider.SetError(txt_password, Constants.INVALID_PASSWORD);
                }
            }
        }
    }
}
