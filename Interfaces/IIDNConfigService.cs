using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardX.Enums;

namespace GuardX.Interfaces
{
    public interface IIDNConfigService
    {
        public EResult CreateIDNFile(string filePath);

        public EIDNFileResults IsValidFormat(string filePath);

        public string GetsAppIdentifier();

        public string GetsCreatedAt();
    }
}
