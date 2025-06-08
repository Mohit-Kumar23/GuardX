using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        }

    }
}
