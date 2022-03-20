using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

namespace Monopoly.Net
{

    public class PacketSocket
    {

        private string loc;
        public WebSocket Sock { get; private set; }
        private bool open, error;

        public static PacketSocket CreateSocket(string address, int port)
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
            string loc = string.Format("ws://{0}:{1}", address, port);
            return new PacketSocket(loc);
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
