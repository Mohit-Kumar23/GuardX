using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.Extensions.Logging;

namespace GuardX.BLServices
{
    public class GxVisibilityService : IVisibilityService
    {
        //Injection of service
        private readonly ILogger<GxVisibilityService> _logger;
        public GxVisibilityService(ILogger<GxVisibilityService> logger) 
        { 
            _logger = logger;
        }

        /// <summary>
        /// Set the Hidden Flag for Files and Directories
        /// Files and Directories should not be visible even with on enabling 'Show Hidden items' from file explorer
        /// </summary>
        /// <returns></returns>
        public EResult Hide()
        {
            EResult eResult = EResult.OK;

            try
            {
                //Get all the Files and Directories
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string[] files = Directory.GetFiles(currentDirectory);
                string[] folders = Directory.GetDirectories(currentDirectory);

                /*Hide all the files except the GuardX.exe, guardx_config.idn and GuardX_log file and such that they are not visible
                  even on enabling 'Show hidden items' in file explorer.
                 */
                foreach (string file in files)
                {
                    if(!Path.GetFileName(file).Equals(Constants.APP_NAME_EXE) && 
                       !Path.GetFileName(file).Equals(Constants.IDN_FILE_NAME) &&
                       !Path.GetFileName(file).Contains(Constants.GUARDX_LOG_SUBSTRING))
                    {
                        File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.Hidden | FileAttributes.System);
                    }
                }

                //Hide all the directories and such that they are not visible even on enabling 'Show hidden items' in file explorer.
                foreach (string dir in folders)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Attributes |= FileAttributes.Hidden | FileAttributes.System;
                }

            }
            catch(Exception ex) 
            {
                _logger.LogError($"FILES_HIDE_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }
            return EResult.OK;
        }

        /// <summary>
        /// UnSet the Hidden attribute for the files to make them again visible.
        /// </summary>
        /// <returns>EResult</returns>
        public EResult UnHide()
        {
            EResult eResult = EResult.OK;

            try
            {
                //Get all the Files and Directories
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string[] files = Directory.GetFiles(currentDirectory);
                string[] folders = Directory.GetDirectories(currentDirectory);

                //Unhide all the files except the GuardX.exe, guardx_config.idn and GuardX_log file
                foreach (string file in files)
                {
                    if (!Path.GetFileName(file).Equals(Constants.APP_NAME_EXE) &&
                       !Path.GetFileName(file).Equals(Constants.IDN_FILE_NAME) &&
                       !Path.GetFileName(file).Contains(Constants.GUARDX_LOG_SUBSTRING))
                    {
                        File.SetAttributes(file, File.GetAttributes(file) & ~(FileAttributes.Hidden | FileAttributes.System));
                    }
                }
                //Unhide all the directories
                foreach (string dir in folders)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Attributes &= ~(FileAttributes.Hidden | FileAttributes.System);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"FILES_UNHIDE_FAILED_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }
            return EResult.OK;
        }
    }
}
