using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardX.Enums
{
    public enum EResult
    {
        OK = 0, ERROR = 1, CANCEL = 2
    }

    public enum ERegistryResults
    {
        RegRequired = 0,
        AlreadyReg = 1,
        InvalidFormat = 2,
        NonUniqueName = 3
    }
}
