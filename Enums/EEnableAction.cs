using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardX.Enums
{
    public enum EEnableAction
    {
        DisableAll = 0x00,
        HideAction = 0x01,
        UnHideAction = 0x02,
        ProfileSetup = 0x04,
        DeleteProfile = 0x08,
        ForgotPwd = 0x10
    }
}
