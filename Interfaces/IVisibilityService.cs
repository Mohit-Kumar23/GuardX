using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardX.Enums;

namespace GuardX.Interfaces
{
    public interface IVisibilityService
    {
        public EResult Hide();

        public EResult UnHide();
    }
}
