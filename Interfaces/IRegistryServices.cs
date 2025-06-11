using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardX.Enums;

namespace GuardX.Interfaces
{
    public interface IRegistryServices
    {
        public ERegistryResults CheckApplicationRegistryAndUniqueness();

        public EResult RegisterApplication();

        public void UnregisterApplication();

        public void RegisterUserDetails();

        public bool IsProfileCreated();

        public String GetProfileName();

        public String GetProfileEmail();

        public EResult CreateProfile(String profileName, String profileEmail);
    }
}
