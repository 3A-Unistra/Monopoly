using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using NativeWebSocket;

namespace Monopoly.Net
{

    public class PacketSocket
    {

        private string loc;
        public WebSocket Sock { get; private set; }
        private bool open, error;

        public static PacketSocket CreateSocket(string address, int port,
                                                bool secure)
        {
            return CreateSocket(address, port, null, secure);
        }

        public static PacketSocket CreateSocket(string address, int port,
            Dictionary<string, string> paramsdic, bool secure)
        {
            if (address == null || address.Equals(""))
            {
                Debug.LogError("Cannot create socket with empty address.");
                return null;
            }
            else if (port < 0 || port > 65535)
            {
                Debug.LogError(
                    "Cannot create socket with invalid port number.");
                return null;
            }
            StringBuilder sb = new StringBuilder();
            if (paramsdic != null)
            {
                int i = 0;
                foreach (string key in paramsdic.Keys)
                {
                    if (i == 0)
                        sb.Append("?");
                    else
                        sb.Append("&");
                    string val = paramsdic[key];
                    sb.Append(string.Format("{0}={1}", key, val));
                }
            }
            string protocol = secure ? "wss" : "ws";
            string loc = string.Format("{0}://{1}:{2}/{3}",
                                       protocol, address, port, sb.ToString());
            return CreateSocket(loc);
        }

        public static PacketSocket CreateSocket(string loc)
        {
            if (loc == null || loc.Equals(""))
            {
                Debug.LogError("Cannot create socket with empty URL.");
                return null;
            }
            return new PacketSocket(loc);
        }

        private PacketSocket(string loc)
        {
            this.loc = loc;
            open = error = false;
            Sock = new WebSocket(loc);
            Sock.OnOpen += () =>
            {
                Debug.Log(string.Format("WebSocket opened at '{0}'", loc));
                open = true;
            };
            Sock.OnError += (e) =>
            {
                /* TODO: Implement timeout or error limit? */
                Debug.LogError(
                    string.Format("WebSocket error at '{0}': {1}", loc, e));
                open = false;
                error = true;
            };
            Sock.OnClose += (e) =>
            {
                Debug.Log(string.Format("WebSocket closed at '{0}'", loc));
                open = false;
            };
        }

        public async void Connect()
        {
            await Sock.Connect();
        }

        public override string ToString()
        {
            return string.Format("PacketSocket<{0}>", loc);
        }

    }

}
