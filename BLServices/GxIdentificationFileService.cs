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

namespace GuardX.BLServices
{
    internal class GxIdentificationFileService : IIdentificationFileService
    {
       private readonly IIDNConfigService _idnConfigService;
       public GxIdentificationFileService(IIDNConfigService idnConfigService)
        {
            _idnConfigService = idnConfigService;
        }

        public bool IsIDNFilePresentOrFormatted()
        {
            bool bRetVal = false;
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = currentPath + "\\" + Constants.IDN_FILE_NAME;

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
                    //TODO: On Error, show some message
                }
            }
            else
            {
                bool bValidFormat = _idnConfigService.IsValidFormat(filePath);

                if (!bValidFormat)
                {
                    DialogResult result = MessageBox.Show(String.Format(Constants.FILE_NON_VALID_FORMAT_MESSAGE, Constants.IDN_FILE_NAME), Constants.ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
