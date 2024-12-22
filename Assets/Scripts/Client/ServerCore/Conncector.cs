using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Connector
    {
        Func<Session> m_SessionFactory;
        SocketAsyncEventArgs m_ConnectArgs = new SocketAsyncEventArgs();

        // 이벤트 등록 소켓 임시 생성 후 세션에게 넘겨줌
        public void Connect(EndPoint endPoint, Func<Session> SessionFunc)
        {
            m_SessionFactory += SessionFunc;
            m_ConnectArgs.Completed += OnConnectCompleted;

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_ConnectArgs.UserToken = socket;
            m_ConnectArgs.RemoteEndPoint = endPoint;
            RegisterConnect();
        }

        public void RegisterConnect()
        {
            Socket socket = m_ConnectArgs.UserToken as Socket;

            if (socket == null) return;
 
            bool pending = socket.ConnectAsync(m_ConnectArgs);
            if (pending == false)
                OnConnectCompleted(null, m_ConnectArgs);
        }

        public void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Console.WriteLine("서버와 연결에 성공");
                Session m_Session = m_SessionFactory.Invoke();
                m_Session.Init(args.UserToken as Socket);
                m_Session.Start();
                //m_Session.OnConnect();
            }
            else
            {
                Console.WriteLine($"Connector : {args.SocketError}");
            }
        }
    }
}
