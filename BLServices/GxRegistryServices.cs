using GuardX.Common;
using GuardX.Enums;
using GuardX.Helper;
using GuardX.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace GuardX.BLServices
{
    internal class GxRegistryServices : IRegistryServices
    {
        private readonly IIDNConfigService _idnConfigService;
        private readonly ILogger<GxRegistryServices> _logger;

        private const string BASE_KEY_PATH = "SOFTWARE";
        private readonly RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
        private RegistryKey applicationSubKey;
        private RegistryKey generalApplicationSubKey;

        public GxRegistryServices(IIDNConfigService idnConfigService,ILogger<GxRegistryServices> logger)
        {
            _idnConfigService = idnConfigService;
            _logger = logger;
        }

        public ERegistryResults CheckApplicationRegistryAndUniqueness()
        {
            ERegistryResults eResult = ERegistryResults.AlreadyReg;

            generalApplicationSubKey = baseKey.OpenSubKey(GetApplicationSubKeyaPathOfGeneral());
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
                applicationSubKey.SetValue(GuardX.Common.Constants.INITIALIZATION_VECTOR, EncryptionDecryptionService.GetIVForAES(), RegistryValueKind.Binary);
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
                var regCreatedTime = GetCreateAt();
                if(regCreatedTime != null && regCreatedTime != "" && regCreatedTime.Equals(_idnConfigService.GetsCreatedAt()))
                {
                    bResult = true;
                }
            }
            return bResult;
        }

        private EResult UpdateRegistryValue(string plainText="", string cipherText="")
        {
            EResult eResult = EResult.OK;

            try
            {
                if (!String.IsNullOrEmpty(plainText) && !String.IsNullOrEmpty(cipherText))
                {
                    applicationSubKey = baseKey.OpenSubKey(GetApplicationSubKeyaPath(), writable: true);
                    if (applicationSubKey != null)
                    {
                        applicationSubKey.SetValue(GuardX.Common.Constants.CIPHER_TEXT, cipherText);
                        applicationSubKey.SetValue(GuardX.Common.Constants.UNIQUE_SENTENCE, plainText);
                    }
                }
            }
            catch(Exception ex)
            {
                //TODO: Log Error here
                eResult = EResult.ERROR;
            }

            return eResult;
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
                if (!String.IsNullOrEmpty(GetCipherText()))
                    if (!String.IsNullOrEmpty(GetUniqueSentence()))
                        if (!String.IsNullOrEmpty(GetCreateAt()))
                            if (GetIV() != null)
                            {
                                bResult = true;
                                EncryptionDecryptionService.SetIVForAES(GetIV());
                            }
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

        public EResult UpdateProfile(String profilePwd, String profileUniqueText)
        {
            EResult eResult = EResult.ERROR;

            EncryptionDecryptionService.SetIVForAES(GetIV());

            string cipherText = EncryptionDecryptionService.Encrypt(profileUniqueText, profilePwd);

            eResult = UpdateRegistryValue(profileUniqueText, cipherText);

            return eResult;
        }

        private String GetCreateAt()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.CREATED_AT).ToString();
        }
        
        private String GetCipherText()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.CIPHER_TEXT).ToString();
        }
        
        public String GetUniqueSentence()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.UNIQUE_SENTENCE).ToString();
        }
        
        private byte[] GetIV()
        {
            return (byte[])applicationSubKey.GetValue(GuardX.Common.Constants.INITIALIZATION_VECTOR);
        }

        public EResult ValidatePassword(string password)
        {
            EResult result = EResult.ERROR;

            string protectedText = GetUniqueSentence();

            string decryptedText = EncryptionDecryptionService.Decrypt(GetCipherText(), password);

            if(protectedText.Equals(decryptedText))
            {
                result = EResult.OK;
            }

            return result;
        }

        public EResult DeleteDirectoryProfile()
        {
            EResult eResult = EResult.OK;
            try
            {
                baseKey.DeleteSubKey(GetApplicationSubKeyaPath());
            }
            catch (Exception ex)
            {
                //TODO: Log Here
                eResult = EResult.ERROR;
            }

            return eResult;
        }
    }
}
