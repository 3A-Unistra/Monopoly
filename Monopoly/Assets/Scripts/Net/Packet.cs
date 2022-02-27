/*
 * Packet.cs
 * Generic networking packet parent type.
 * 
 * Date created : 19/02/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Monopoly.Net
{

    /**
     * <summary>
     *     Abstract packet class from which all client/server packets must
     *     inherit.
     * </summary>
     */
    [JsonConverter(typeof(PacketConverter))]
    public abstract class Packet
    {

        /**
         * <summary>
         *     The class-name of a given packet. As all packets are serialisable
         *     objects that are translated to and from JSON, this field is
         *     required to determine the type of any given packet.
         * </summary>
         */
        [JsonProperty("name")]
        public string Name
        {
            get;
            private set;
        }

        /**
         * <summary>
         *     Creates a new instance of a packet with a given JSON name.
         * </summary>
         * <param name="name">
         *     The name of a given packet. See <see cref="Name"/> for more information.
         * </param>
         */
        public Packet(string name)
        {
            this.Name = name;
        }

        /**
         * <summary>
         *     Serialises a packet into its JSON equivalent string for sending over to
         *     the server.
         * </summary>
         * <returns>
         *     The packet as a serialised JSON object.
         * </returns>
         */
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        /**
         * <summary>
         *     Deserialises a JSON string into the correct packet type.
         * </summary>
         * 
         * For deserialisation to work correctly, a given JSON string must contain the
         * field 'name' which corresponds to the packet name to be deserialised into.
         * 
         * The returned packet will be of the generic type Packet, but the underlying
         * type will be correct.
         * 
         * <exception cref="JsonException">
         *     If the deserialisation fails for a reason releted to the parsing of the
         *     JSON string.
         * </exception>
         * <param name="json">
         *     The JSON string to deserialise into a packet.
         * </param>
         * <returns>
         *     The deserialised packet from a given JSON string.
         * </returns>
         */
        public static Packet Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject
                    <Packet>(json, new PacketConverter());
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
