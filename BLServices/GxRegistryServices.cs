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

        /// <summary>
        /// To Check if application profile is registered in the Registry.
        /// If Registered, is it unique in the whole registry and if unique does the format is correct.
        /// If Not, then return the result as registration required
        /// </summary>
        /// <returns>
        /// Return registry status as per the condition
        /// </returns>
        public ERegistryResults CheckApplicationRegistryAndUniqueness()
        {
            ERegistryResults eResult = ERegistryResults.AlreadyReg;

            //Open the both GuardX_General and GuardX_<AppIdentifier>
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

        /// <summary>
        /// Create the Application profile in the registry named GuardX_<AppIdentifier> with basic information
        /// </summary>
        /// <returns>
        /// EResult
        /// </returns>
        public EResult RegisterApplication()
        {
            EResult eResult = EResult.OK;

            try
            {
                applicationSubKey = baseKey.CreateSubKey(GetApplicationSubKeyaPath());
                if (applicationSubKey != null)
                {
                    applicationSubKey.SetValue(GuardX.Common.Constants.CREATED_AT, _idnConfigService.GetsCreatedAt());
                    applicationSubKey.SetValue(GuardX.Common.Constants.INITIALIZATION_VECTOR, EncryptionDecryptionService.GetIVForAES(), RegistryValueKind.Binary);
                    applicationSubKey.SetValue(GuardX.Common.Constants.UNIQUE_SENTENCE, "");
                    applicationSubKey.SetValue(GuardX.Common.Constants.CIPHER_TEXT, "");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"REGISTER_APPLICATION_PROFILE_CREATION_ERROR_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }        

            return eResult;
        }

        /// <summary>
        /// In registry two keys can't exist of same name. So, for unqiueness we also see if the Creation Time
        /// of registry and file is same or not.
        /// </summary>
        /// <returns>
        /// Boolean result
        /// </returns>
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

        /// <summary>
        /// Update the registry with Cipher Text and ProtectedText.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cipherText"></param>
        /// <returns>
        /// EResult
        /// </returns>
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
                _logger.LogError($"UPDATE_REGISTRY_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }

            return eResult;
        }
        
        /// <summary>
        /// Gets the Registry Path for Application Profile
        /// </summary>
        /// <returns></returns>
        private string GetApplicationSubKeyaPath()
        {
            return BASE_KEY_PATH + "\\" + GuardX.Common.Constants.APP_NAME + "_" + _idnConfigService.GetsAppIdentifier();
        }

        /// <summary>
        /// Gets the Registry Path for the General Application
        /// </summary>
        /// <returns></returns>
        private string GetApplicationSubKeyaPathOfGeneral()
        {
            return BASE_KEY_PATH + "\\" + GuardX.Common.Constants.APP_NAME + "_" + GuardX.Common.Constants.GENERAL;
        }

        /// <summary>
        /// To check if all the fields within the application profile is in correct format or not.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if the general profile is already created or not.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create the general profile GuardX_General if not created yet in the registry.
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="profileEmail"></param>
        /// <returns></returns>
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
                _logger.LogError($"GENERAL_PROFILE_CREATION_ERROR_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }

            return eResult;

        }


        /// <summary>
        /// Gets the User name from the registry
        /// </summary>
        /// <returns></returns>
        public string GetProfileName()
        {
            return generalApplicationSubKey.GetValue(GuardX.Common.Constants.PROFILE_NAME).ToString();
        }

        /// <summary>
        /// Gets the User email from the registry
        /// </summary>
        /// <returns></returns>
        public string GetProfileEmail()
        {
            return generalApplicationSubKey.GetValue(GuardX.Common.Constants.PROFILE_EMAIL).ToString();
        }

        /// <summary>
        /// Update the profile for protected text and cipher text in the registry
        /// </summary>
        /// <param name="profilePwd"></param>
        /// <param name="profileUniqueText"></param>
        /// <returns>
        /// EResult
        /// </returns>
        public EResult UpdateProfile(String profilePwd, String profileUniqueText)
        {
            EResult eResult = EResult.ERROR;

            EncryptionDecryptionService.SetIVForAES(GetIV());

            string cipherText = EncryptionDecryptionService.Encrypt(profileUniqueText, profilePwd);

            eResult = UpdateRegistryValue(profileUniqueText, cipherText);

            return eResult;
        }

        /// <summary>
        /// Gets the CreatedTime from the registry
        /// </summary>
        /// <returns></returns>
        private String GetCreateAt()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.CREATED_AT).ToString();
        }
        
        /// <summary>
        /// Gets the Cipher text from the registry
        /// </summary>
        /// <returns></returns>
        private String GetCipherText()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.CIPHER_TEXT).ToString();
        }
        
        /// <summary>
        /// Gets the protected text from the registry
        /// </summary>
        /// <returns></returns>
        public String GetUniqueSentence()
        {
            return applicationSubKey.GetValue(GuardX.Common.Constants.UNIQUE_SENTENCE).ToString();
        }
        
        /// <summary>
        /// Gets the Intialization vector of AES-128 from the registry
        /// </summary>
        /// <returns></returns>
        private byte[] GetIV()
        {
            return (byte[])applicationSubKey.GetValue(GuardX.Common.Constants.INITIALIZATION_VECTOR);
        }

        /// <summary>
        /// Decrypt the cipher text from the user input password and compare it with the protected text from the registry.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>EResult</returns>
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

        /// <summary>
        /// Delete the Application Profile from the registry
        /// </summary>
        /// <returns>EResult</returns>
        public EResult DeleteRegistryProfile()
        {
            EResult eResult = EResult.OK;
            try
            {
                baseKey.DeleteSubKey(GetApplicationSubKeyaPath());
            }
            catch (Exception ex)
            {
                _logger.LogError($"DELETE_PROFILE_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }

            return eResult;
        }
    }
}
