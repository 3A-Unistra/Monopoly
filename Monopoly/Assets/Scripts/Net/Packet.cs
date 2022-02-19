using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Monopoly.Net
{

    public abstract class Packet
    {

        [JsonProperty("packet_name")]
        private string name;

        public Packet(string name)
        {
            this.name = name;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Deserialize()
        {
            return "";
        }

    }

}
