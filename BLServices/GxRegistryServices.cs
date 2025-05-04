using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardX.Enums;
using GuardX.Interfaces;

namespace GuardX.BLServices
{
    internal class GxRegistryServices : IRegistryServices
    {
        private readonly IIDNConfigService _idnConfigService;

        public GxRegistryServices(IIDNConfigService idnConfigService)
        {
            _idnConfigService = idnConfigService;
        }

        public ERegistryResults CheckApplicationRegistryAndUniqueness()
        {
            ERegistryResults eResult = ERegistryResults.AlreadyReg;
            return eResult;
        }

        public EResult RegisterApplication()
        {
            EResult eResult = EResult.OK;
            return eResult;
        }

        public void UnregisterApplication()
        {

        }

        public void RegisterUserDetails()
        {

        }
    }
}
