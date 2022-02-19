using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using Monopoly.Net.Packets;

namespace Monopoly.Net
{

    /*
     * See: https://stackoverflow.com/a/30579193
     */
    public class PacketConverter : JsonConverter
    {

        private class PacketClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter
                (Type objectType)
            {
                if (typeof(Packet).IsAssignableFrom(objectType) &&
                    !objectType.IsAbstract)
                    /* pretend TableSortRuleConvert is not specified
                       (thus avoiding a stack overflow) */
                    return null;
                return base.ResolveContractConverter(objectType);
            }
        }

        private static JsonSerializerSettings SpecifiedSubclassConversion =
            new JsonSerializerSettings()
            {
                ContractResolver = new PacketClassConverter()
            };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Packet));
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string name = jo["packet_name"].Value<string>();
            switch (name)
            {
            case "AppletPrepare":
                return JsonConvert.DeserializeObject<PacketAppletPrepare>(
                    jo.ToString(), SpecifiedSubclassConversion
                );
            case "AppletReady":
                return JsonConvert.DeserializeObject<PacketAppletReady>(
                    jo.ToString(), SpecifiedSubclassConversion
                );
            default:
                break;
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            /* will not occur because CanWrite = false */
            throw new NotImplementedException();
        }

    }

}
