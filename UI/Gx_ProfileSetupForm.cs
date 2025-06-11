using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuardX.Enums;
using GuardX.Interfaces;

namespace GuardX.UI
{
    public partial class Gx_ProfileSetupForm : Form
    {
        private readonly IRegistryServices _registryServices;
        public Gx_ProfileSetupForm(IRegistryServices registryServices)
        {
            _registryServices = registryServices;

            InitializeComponent();
            FillProfileViews();
        }

        private void FillProfileViews()
        {
            bool bResult = _registryServices.IsProfileCreated();
            if(bResult)
            {
                txtBx_name.Text = _registryServices.GetProfileName();
                txtBx_name.Enabled = false;
                txtBx_email.Text = _registryServices.GetProfileEmail();
                txtBx_email.Enabled = false;    
            }
        }

        private void btn_save_click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBx_name.Text) && !String.IsNullOrEmpty(txtBx_email.Text)
                && !String.IsNullOrEmpty(txtBx_pwd.Text) && !String.IsNullOrEmpty(txtBx_uniqueText.Text))
            {
                EResult result = EResult.OK;

                if(!_registryServices.IsProfileCreated())
                {
                    result = _registryServices.CreateProfile(txtBx_name.Text, txtBx_email.Text);
                }
                if (result == EResult.OK)
                {
                    //Encryption Alogrithm to take the password and unique string to store the plain text and cipher text
                }
                if(result == EResult.OK)
                {
                    Close();
                }
            }
            else
            {
                MessageBox.Show(GuardX.Common.Constants.FILL_ALL_FIELDS, GuardX.Common.Constants.ERROR, MessageBoxButtons.OK);
            }
        }

        private void btn_cancel_click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
