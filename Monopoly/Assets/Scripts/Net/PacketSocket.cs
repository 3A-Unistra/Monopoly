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
                                                bool lobby, bool secure)
        {
            return CreateSocket(address, port, null, lobby, secure);
        }

        public static PacketSocket CreateSocket(string address, int port,
            Dictionary<string, string> paramsdic, bool lobby, bool secure)
        {
            if (port < 0 || port > 65535)
            {
                Debug.LogError(
                    "Cannot create socket with invalid port number.");
                return null;
            }
            return CreateSocket(string.Format("{0}:{1}", address, port),
                                paramsdic, lobby, secure);
        }

        public static PacketSocket CreateSocket(string addressport,
            Dictionary<string, string> paramsdic, bool lobby, bool secure)
        {
            addressport = addressport.Trim();
            if (addressport == null || addressport.Equals(""))
            {
                Debug.LogError("Cannot create socket with empty address.");
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
            string type = lobby ? "lobby" : "game";
            string loc = string.Format("{0}://{1}/ws/{2}{3}",
                                       protocol, addressport, type,
                                       sb.ToString());
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
            Dictionary<string, string> headers =
                new Dictionary<string, string>();
            // FIXME: FETCH CORRECT ORIGIN
            //headers.Add("Origin", "http://192.168.43.2");
            try
            {
                Sock = new WebSocket(loc, headers);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
                error = true;
                Sock = null;
                return;
            }
            Sock.OnOpen += () =>
            {
                Debug.Log(string.Format("WebSocket opened at '{0}'", loc));
                open = true;
            };
#if UNITY_EDITOR
            Sock.OnMessage += (e) =>
            {
                Debug.Log(string.Format("WebSocket message: {0}",
                          Encoding.UTF8.GetString(e)));
            };
#endif
            Sock.OnError += (e) =>
            {
                /* TODO: Implement timeout or error limit? */
                Debug.LogWarning(
                    string.Format("WebSocket error at '{0}': {1}", loc, e));
                open = false;
                error = true;
            };
            Sock.OnClose += (e) =>
            {
                Debug.Log(
                    string.Format("WebSocket closed at '{0}' with code {1}",
                    loc, e));
                open = false;
            };
        }

        public async void Connect()
        {
            Debug.Log("Websocket connecting...");
            if (Sock != null)
                await Sock.Connect();
        }

        public async void Close()
        {
            Debug.Log("Websocket closing...");
            if (Sock != null)
                await Sock.Close();
        }

        public override string ToString()
        {
            return string.Format("PacketSocket<{0}>", loc);
        }

        public bool HasError()
        {
            return error;
        }

        public bool IsOpen()
        {
            return open;
        }

    }

}
