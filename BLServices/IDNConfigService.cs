using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using GuardX.Model;

namespace GuardX.BLServices
{
    internal class IDNConfigService : IIDNConfigService
    {
        private IDNConfig idnConfig;

        public IDNConfigService()
        {
            idnConfig = new IDNConfig();
        }
        public EResult CreateIDNFile(string filePath)
        {
            EResult retVal = EResult.OK;
            try
            {
                idnConfig.AppIdentifier = Constants.DEFAULT_INIT_APP_IDENTIFIER;
                idnConfig.CreatedAt = DateTime.Now.ToString();
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string serialized = JsonSerializer.Serialize(idnConfig, options);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(serialized);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log Here
                retVal = EResult.ERROR;
            }
            return retVal;
        }

        public EIDNFileResults IsValidFormat(string filePath)
        {
            EIDNFileResults eResult = EIDNFileResults.Valid;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string fileContent = Encoding.UTF8.GetString(buffer);
                    
                    idnConfig = JsonSerializer.Deserialize<IDNConfig>(fileContent);

                    if (idnConfig != null)
                    {
                        if(idnConfig.AppIdentifier.Equals(Constants.DEFAULT_INIT_APP_IDENTIFIER))
                        {
                            eResult = EIDNFileResults.InvalidIdentifier;
                            return eResult;
                        }

                        var fileCreationTime = File.GetCreationTime(filePath);
                        if (!idnConfig.CreatedAt.Equals(fileCreationTime.ToString()))
                        {
                            eResult = EIDNFileResults.CreationTimeEditedError;
                        }
                    }
                    else
                    {
                        eResult = EIDNFileResults.OtherError;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Log Here
                eResult = EIDNFileResults.OtherError;
            }
            return eResult;
        }

        public string GetsAppIdentifier()
        {
            return idnConfig.AppIdentifier;
        }

        public string GetsCreatedAt()
        {
            return idnConfig.CreatedAt;
        }

        public EResult DeleteIDNConfigFile(string filePath)
        {
            EResult eResult = EResult.OK;
            try
            {
                File.Delete(filePath);
            }
            catch(Exception ex)
            {
                //TODO: Log Here
                eResult = EResult.ERROR;
            }
            return eResult;
        }
    }
}
