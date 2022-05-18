using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using NativeWebSocket;

using Monopoly.Runtime;

namespace Monopoly.Net
{

    public class PacketSocket
    {

        private string loc;
        public WebSocket Sock { get; private set; }
        private bool open, error, tlserror;

        public static string SpliceAddress(string addr, int port)
        {
            // splice together the address with the port
            // foo.bar port 80 becomes foo.bar:80
            // foo.bar/bam port 80 becomes foo.bar:80/bam
            string[] addressPieces = addr.Split('/');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < addressPieces.Length; ++i)
            {
                sb.Append(addressPieces[i]);
                if (i == 0)
                    sb.Append(string.Format(":{0}", port));
                if (i < addressPieces.Length - 1)
                    sb.Append('/');
            }
            return sb.ToString();
        }

        public static PacketSocket CreateSocket(string address, int port,
                                                string gameToken, bool secure)
        {
            return CreateSocket(address, port, null, gameToken, secure);
        }

        public static PacketSocket CreateSocket(string address, int port,
            Dictionary<string, string> paramsdic, string gameToken, bool secure)
        {
            if (port < 0 || port > 65535)
            {
                Debug.LogError(
                    "Cannot create socket with invalid port number.");
                return null;
            }
            return CreateSocket(SpliceAddress(address, port),
                                paramsdic, gameToken, secure);
        }

        public static PacketSocket CreateSocket(string addressport,
            Dictionary<string, string> paramsdic, string gameToken, bool secure)
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
            string type = gameToken == null ? "lobby" :
                          string.Format("game/{0}", gameToken);
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

        public async void SendPacket(Packet packet)
        {
            string serial = packet.Serialize();
#if UNITY_EDITOR || UNITY_WEBGL
            Debug.Log("WebSocket send: " + serial);
#endif
            try
            {
                await Sock.SendText(serial);
            }
            catch (System.Exception)
            {
                //Debug.LogException(e);
                Debug.LogWarning("WebSocket died. Will now quit the game...");
                if (ClientGameState.current != null)
                    ClientGameState.current.Crash();
                else
                    ClientLobbyState.current.Crash();
            }
        }

        private PacketSocket(string loc)
        {
            this.loc = loc;
            open = error = tlserror = false;
            Dictionary<string, string> headers =
                new Dictionary<string, string>();
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
#if UNITY_EDITOR || UNITY_WEBGL
            Sock.OnMessage += (e) =>
            {
                Debug.Log(string.Format("WebSocket recv: {0}",
                          Encoding.UTF8.GetString(e)));
            };
#endif
            Sock.OnError += (e) =>
            {
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
                tlserror = e == WebSocketCloseCode.TlsHandshakeFailure;
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

        public bool HasTLSError()
        {
            return error;
        }

        public bool IsOpen()
        {
            return open;
        }

    }

}
