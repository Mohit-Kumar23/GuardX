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
        private readonly ILogger<GxVisibilityService> _logger;
        public GxVisibilityService(ILogger<GxVisibilityService> logger) 
        { 
            _logger = logger;
        }
        public EResult Hide()
        {
            EResult eResult = EResult.OK;

            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string[] files = Directory.GetFiles(currentDirectory);
                string[] folders = Directory.GetDirectories(currentDirectory);

                foreach(string file in files)
                {
                    if(!Path.GetFileName(file).Equals(Constants.APP_NAME_EXE) && 
                       !Path.GetFileName(file).Equals(Constants.IDN_FILE_NAME))
                    {
                        File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.Hidden | FileAttributes.System);
                    }
                }

                foreach(string dir in folders)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Attributes |= FileAttributes.Hidden | FileAttributes.System;
                }

            }
            catch(Exception ex) 
            {
                //TODO: Log Here
                eResult = EResult.ERROR;
            }
            return EResult.OK;
        }

        public EResult UnHide()
        {
            EResult eResult = EResult.OK;

            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string[] files = Directory.GetFiles(currentDirectory);
                string[] folders = Directory.GetDirectories(currentDirectory);

                foreach (string file in files)
                {
                    if (!Path.GetFileName(file).Equals(Constants.APP_NAME_EXE) &&
                       !Path.GetFileName(file).Equals(Constants.IDN_FILE_NAME))
                    {
                        File.SetAttributes(file, File.GetAttributes(file) & ~(FileAttributes.Hidden | FileAttributes.System));
                    }
                }

                foreach (string dir in folders)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Attributes &= ~(FileAttributes.Hidden | FileAttributes.System);
                }

            }
            catch (Exception ex)
            {
                //TODO: Log Here
                eResult = EResult.ERROR;
            }
            return EResult.OK;
        }
    }
}
