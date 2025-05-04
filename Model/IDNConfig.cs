using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GuardX.Common;

namespace GuardX.Model
{
    public class IDNConfig
    {
        [JsonPropertyName("AppIdentifier")]
        public string AppIdentifier { get; set; }

        [JsonPropertyName("CreatedAt")]
        public string CreatedAt { get; set; }

    }
}
