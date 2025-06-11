using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace GuardX.BLServices
{
    internal class GxRegistryServices : IRegistryServices
    {
        private readonly IIDNConfigService _idnConfigService;
        private const string BASE_KEY_PATH = "SOFTWARE";
        private readonly RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
        private RegistryKey applicationSubKey;
        private RegistryKey generalApplicationSubKey;
        private RegistryKey userProfileSubKey;

        public GxRegistryServices(IIDNConfigService idnConfigService)
        {
            _idnConfigService = idnConfigService;
        }

        public ERegistryResults CheckApplicationRegistryAndUniqueness()
        {
            ERegistryResults eResult = ERegistryResults.AlreadyReg;

            applicationSubKey = baseKey.OpenSubKey(GetApplicationSubKeyaPath());

            if (applicationSubKey == null)
            {
                eResult = ERegistryResults.RegRequired;
            }
            else 
            {
                if (IsAppIdentifierUniqueInRegistry())
                {
                    if (!IsRegistryInCorrectFormat())
                    {
                        eResult = ERegistryResults.UpdateRequired;
                    }
                }
                else 
                {
                    eResult = ERegistryResults.NonUniqueName;
                }
            }

            return eResult;
        }

        public EResult RegisterApplication()
        {
            EResult eResult = EResult.OK;

            applicationSubKey = baseKey.CreateSubKey(GetApplicationSubKeyaPath());
            if (applicationSubKey != null)
            {
                applicationSubKey.SetValue(GuardX.Common.Constants.CREATED_AT, _idnConfigService.GetsCreatedAt());
                applicationSubKey.SetValue(GuardX.Common.Constants.UNIQUE_SENTENCE, "");
                applicationSubKey.SetValue(GuardX.Common.Constants.CIPHER_TEXT, "");
            }
         

            return eResult;
        }

        private bool IsAppIdentifierUniqueInRegistry()
        {
            bool bResult = false;

            if (applicationSubKey != null) 
            {
                var regCreatedTime = applicationSubKey.GetValue(GuardX.Common.Constants.CREATED_AT).ToString();
                if(regCreatedTime != null && regCreatedTime != "" && regCreatedTime.Equals(_idnConfigService.GetsCreatedAt()))
                {
                    bResult = true;
                }
            }

            return bResult;
        }

        public void UnregisterApplication()
        {

        }

        public void RegisterUserDetails()
        {

        }

        private void UpdateRegistryValue(string plainText="Life is great when krishna is with you", string password = "HiMohit")
        {
            applicationSubKey = baseKey.OpenSubKey(GetApplicationSubKeyaPath(),writable:true);
            if (applicationSubKey != null)
            {
                applicationSubKey.SetValue(GuardX.Common.Constants.CIPHER_TEXT, password);
                applicationSubKey.SetValue(GuardX.Common.Constants.UNIQUE_SENTENCE, plainText);
            }
        }

        private bool DoesRegistryKeyExist()
        {
            bool bRetVal = false;
            return bRetVal;
        }

        private string GetApplicationSubKeyaPath()
        {
            return BASE_KEY_PATH + "\\" + GuardX.Common.Constants.APP_NAME + "_" + _idnConfigService.GetsAppIdentifier();
        }

        private string GetApplicationSubKeyaPathOfGeneral()
        {
            return BASE_KEY_PATH + "\\" + GuardX.Common.Constants.APP_NAME + "_" + GuardX.Common.Constants.GENERAL;
        }

        private bool IsRegistryInCorrectFormat()
        {
            bool bResult = false;
            
            if (applicationSubKey != null)
            {
                if(!String.IsNullOrEmpty(applicationSubKey.GetValue(GuardX.Common.Constants.CIPHER_TEXT).ToString()))
                    if(!String.IsNullOrEmpty(applicationSubKey.GetValue(GuardX.Common.Constants.UNIQUE_SENTENCE).ToString()))
                        if(!String.IsNullOrEmpty(applicationSubKey.GetValue(GuardX.Common.Constants.CREATED_AT).ToString()))
                            bResult = true;
            }

            return bResult;
        }

        public bool IsProfileCreated()
        {
            bool bResult = false;

            if(generalApplicationSubKey == null)
            {
                generalApplicationSubKey = baseKey.OpenSubKey(GetApplicationSubKeyaPathOfGeneral());
                if (generalApplicationSubKey != null)
                {
                    bResult = true;
                }
            }
            else
            {
                bResult = true;
            }

            return bResult;
        }

        public EResult CreateProfile(String profileName, String profileEmail)
        {
            EResult eResult = EResult.OK;

            try
            {
                generalApplicationSubKey = baseKey.CreateSubKey(GetApplicationSubKeyaPathOfGeneral());
                if (generalApplicationSubKey != null)
                {
                    generalApplicationSubKey.SetValue(GuardX.Common.Constants.PROFILE_NAME, profileName);
                    generalApplicationSubKey.SetValue(GuardX.Common.Constants.PROFILE_EMAIL,profileEmail);
                }

            }
            catch (Exception ex)
            {
                //TODO: Log Error here
                eResult = EResult.ERROR;
            }

            return eResult;

        }

        public string GetProfileName()
        {
            return generalApplicationSubKey.GetValue(GuardX.Common.Constants.PROFILE_NAME).ToString();
        }

        public string GetProfileEmail()
        {
            return generalApplicationSubKey.GetValue(GuardX.Common.Constants.PROFILE_EMAIL).ToString();
        }
    }
}
