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
using Microsoft.Extensions.Logging;
using NLog;

namespace GuardX.BLServices
{
    internal class IDNConfigService : IIDNConfigService
    {
        private IDNConfig idnConfig;
        private readonly ILogger<IDNConfigService> _logger;

        public IDNConfigService(ILogger<IDNConfigService> logger)
        {
            _logger = logger;
            idnConfig = new IDNConfig();
        }

        /// <summary>
        /// Create the guardx_config.idn file in the current directory
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public EResult CreateIDNFile(string filePath)
        {
            EResult retVal = EResult.OK;
            try
            {
                idnConfig.AppIdentifier = Constants.DEFAULT_INIT_APP_IDENTIFIER;
                idnConfig.CreatedAt = DateTime.Now.ToString();
                //Pretify the JSON result.
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
                _logger.LogError($"IDN_FILE_NOT_CREATED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                retVal = EResult.ERROR;
            }
            return retVal;
        }

        /// <summary>
        /// Check if the AppIdentifer Name is still not the default string and the CreatedTime has not been altered
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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
                        //Check if the AppIdentifier is still not the default string.
                        if(idnConfig.AppIdentifier.Equals(Constants.DEFAULT_INIT_APP_IDENTIFIER))
                        {
                            eResult = EIDNFileResults.InvalidIdentifier;
                            return eResult;
                        }

                        //Check if CreatedTime is not altered by comparing with File Creation Time.
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
                _logger.LogError($"IDN_VALIDITY_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EIDNFileResults.OtherError;
            }
            return eResult;
        }

        /// <summary>
        /// Gets the AppIdentifier name from the config file.
        /// </summary>
        /// <returns></returns>
        public string GetsAppIdentifier()
        {
            return idnConfig.AppIdentifier;
        }

        /// <summary>
        /// Gets the CreatedAt time from the config file.
        /// </summary>
        /// <returns></returns>
        public string GetsCreatedAt()
        {
            return idnConfig.CreatedAt;
        }

        /// <summary>
        /// Delete the IDN config file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>EResult</returns>
        public EResult DeleteIDNConfigFile(string filePath)
        {
            EResult eResult = EResult.OK;
            try
            {
                File.Delete(filePath);
            }
            catch(Exception ex)
            {
                _logger.LogError($"DELETE_IDN_FILE_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }
            return eResult;
        }
    }
}
