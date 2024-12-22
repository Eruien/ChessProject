using ServerCore;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts
{
    class MainClient : MonoBehaviour
    {
        public static PlayerList g_ClientPlayerList = new PlayerList();

        private void Awake()
        {
            string host = Dns.GetHostName();

            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = IPAddress.Parse("25.31.78.91");
            IPEndPoint endPoint = new IPEndPoint(ipHost.AddressList[0], Global.g_PortNumber);
            // 세션 생성한다음에 메시지 주고 받기 일단 주는것 부터
            Connector connector = new Connector();
            connector.Connect(endPoint, SessionManager.Instance.Create);
        }

        private void Update()
        {
            IPacket packet = PacketQueue.Instance.Pop();

            if (packet != null)
            {
                PacketManager.Instance.HandlePacket(packet.PacketID, SessionManager.Instance.GetServerSession(), packet);
            }
        }
    }
}
