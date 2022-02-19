using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Monopoly.Net
{

    [JsonConverter(typeof(PacketConverter))]
    public abstract class Packet
    {

        [JsonProperty("packet_name")]
        public string Name
        {
            get;
            private set;
        }

        public Packet(string name)
        {
            this.Name = name;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Packet Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Packet>(json,
                                                             new PacketConverter());
            }
            catch (JsonException e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public override string ToString()
        {
            return Serialize();
        }

    }

}
