using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GuardX.Common
{
    public static class Constants
    {
        //Generic Strings
        public static readonly string APP_NAME = "GuardX";
        public static readonly string APP_NAME_EXE = "GuardX.exe";
        public static readonly string ERROR = "Error";
        public static readonly string CREATED_AT = "CreatedAt";
        public static readonly string APPLICATION_ERROR = "Application ran into error. Please contact your developer.";

        //Registry Related Strings
        public static readonly string GENERAL = "General";
        public static readonly string PROFILE_NAME = "Name";
        public static readonly string PROFILE_EMAIL = "Email";
        public static readonly string INVALID_UNIQUE_NAME = "Provided AppIdentified is not unique and is already used by other GuardX application. Please provide a unique AppIdentifier in {0}";
        public static readonly string CIPHER_TEXT = "CipherText";
        public static readonly string UNIQUE_SENTENCE = "Protected Text";

        //IDN File Related Strings
        public static readonly string IDN_FILE_NAME = "guardx_config.idn";
        public static readonly string DEFAULT_INIT_APP_IDENTIFIER = "<Give Unique Name related to this directory>";
        public static readonly string FILE_CREATED_MESSAGE = "Open {0} in current directory and provide unique name under AppIdentifier.";
        public static readonly string FILE_CREATED_TITLE = "IDN File Created";
        public static readonly string FILE_NON_VALID_FORMAT_MESSAGE = "File {0} is not in correct format. Please provide unique name under AppIdentifier.";
        public static readonly string FILE_CREATION_TIME_EDITED_ERROR = "File {0} is not in correct format. Please don't edit the \'CreatedAt\' field.";
        
        //UI Related Strings
        public static readonly string UPDATE_REGX_REQ = "Please setup your profile by clicking \"Profile Setup\" ";
        public static readonly string PROFILE_SETUP_REQ_TITLE = "Profile Setup Required";
        public static readonly string NAME_FIELD_EMPTY_ERROR = "Name field can\'t be empty.";
        public static readonly string EMAIL_FIELD_EMPTY_ERROR = "Email field can\'t be empty.";
        public static readonly string PWD_FIELD_EMPTY_ERROR = "Password field can\'t be empty.";
        public static readonly string OTP_FIELD_EMPTY_ERROR = "OTP field can\'t be empty.";
        public static readonly string SENTENCE_FIELD_EMPTY_ERROR = "Unique Sentence field can\'t be empty.";
        public static readonly string EMAIL_NOT_VERIFIED_ERROR = "Email is not verified";
        public static readonly string INITIALIZATION_VECTOR = "IV";
        public static readonly string INVALID_PASSWORD = "Password is invalid";
        public static readonly string INVALID_OTP = "OTP is invalid";
        public static readonly string OTP_EMAIL_SUBJECT = "OTP Verification from GuardX";
        public static readonly string EMAIL_PURPOSE_SETUP_PROFILE = "Setup your profile";
        public static readonly string EMAIL_PURPOSE_RESET_PROFILE = "Reset your profile";

    }
}
