using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using GuardX.Model;
using Microsoft.Extensions.Logging;

namespace GuardX.BLServices
{
    internal class GxIdentificationFileService : IIdentificationFileService
    {
        //Injection of Services
        private readonly IIDNConfigService _idnConfigService;
        private readonly ILogger<GxIdentificationFileService> _logger;

        public GxIdentificationFileService(IIDNConfigService idnConfigService,ILogger<GxIdentificationFileService> logger)
        {
            _idnConfigService = idnConfigService;
            _logger = logger;
        }

        /// <summary>
        /// Delete the guardx_config.idn file
        /// </summary>
        /// <returns></returns>
        public EResult DeleteIDNFile()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = currentPath + "\\" + Constants.IDN_FILE_NAME;
            return _idnConfigService.DeleteIDNConfigFile(filePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsIDNFilePresentOrFormatted()
        {
            bool bRetVal = false;
            //Gets the current path where from where exe is executed
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = currentPath + "\\" + Constants.IDN_FILE_NAME;

            //Check if file exist or not.
            if (!File.Exists(filePath))
            {
                EResult result = _idnConfigService.CreateIDNFile(filePath);

                if (result == EResult.OK)
                {
                    DialogResult dialogResult = MessageBox.Show(String.Format(Constants.FILE_CREATED_MESSAGE, Constants.IDN_FILE_NAME), Constants.FILE_CREATED_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Cancel)
                    {
                        Environment.Exit(0);
                    }
                }
                else if (result == EResult.ERROR)
                {
                    _logger.LogError("Error during IDN file creation.");
                }
            }
            //Check the format if file is in correct format.
            else
            {
                EIDNFileResults eResult = _idnConfigService.IsValidFormat(filePath);

                if (EIDNFileResults.Valid != eResult)
                {
                    DialogResult result;
                    if (EIDNFileResults.InvalidIdentifier == eResult)
                    {
                        result = MessageBox.Show(String.Format(Constants.FILE_NON_VALID_FORMAT_MESSAGE, Constants.IDN_FILE_NAME), Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (EIDNFileResults.CreationTimeEditedError == eResult)
                    {
                        result = MessageBox.Show(String.Format(Constants.FILE_CREATION_TIME_EDITED_ERROR, Constants.IDN_FILE_NAME), Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        result = MessageBox.Show(Constants.APPLICATION_ERROR, Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (result == DialogResult.OK || result == DialogResult.Cancel)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    bRetVal = true;
                }

            }

            return bRetVal;
        }
    }


}
