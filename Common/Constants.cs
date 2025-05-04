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
        public static readonly string APP_NAME = "GuardX";
        public static readonly string IDN_FILE_NAME = "guardx_config.idn";
        public static readonly string DEFAULT_INIT_APP_IDENTIFIER = "<Give Unique Name related to this directory>";
        public static readonly string FILE_CREATED_MESSAGE = "Open {0} in current directory and provide unique name under AppIdentigfier.";
        public static readonly string FILE_CREATED_TITLE = "IDN File Created";
        public static readonly string ERROR = "Error";
        public static readonly string FILE_NON_VALID_FORMAT_MESSAGE = "File {0} is not in correct format. Please provide unique name under AppIdentifier.";

    }
}
