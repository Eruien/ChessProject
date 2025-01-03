﻿using System;
using System.Net;
using System.Net.Sockets;
using Assets.Scripts;

namespace ServerCore
{
    public class Listener
    {
        Func<Session> m_SessionFactory;
        Socket m_Socket;
        SocketAsyncEventArgs m_ListenArgs = new SocketAsyncEventArgs();

        // 리스닝 소켓 바인딩 리스닝, 이벤트 등록
        public void Init(Func<Session> sessionFunc, int listenCount)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = IPAddress.Parse("25.31.78.91");
            IPEndPoint endPoint = new IPEndPoint(ipHost.AddressList[0], Global.g_PortNumber);
            m_Socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            m_Socket.Bind(endPoint);
            m_Socket.Listen(listenCount);

            m_ListenArgs.Completed += OnAcceptCompleted;
            m_SessionFactory += sessionFunc;
        }

        public void Start()
        {
            RgisterAccept();
        }

        void RgisterAccept()
        {
            m_ListenArgs.AcceptSocket = null;

            bool pending = m_Socket.AcceptAsync(m_ListenArgs);
            if (pending == false)
                OnAcceptCompleted(null, m_ListenArgs);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Console.WriteLine("클라이언트와 연결 성공");

                try
                {
                    Session session = m_SessionFactory.Invoke();
                    session.Init(args.AcceptSocket);
                    session.Start();
                    session.OnConnect();
                    RgisterAccept();
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Listener -> OnAcceptCompleted : {e}");
                }
            }
            else
            {
                Console.WriteLine($"Listener -> OnAcceptCompleted : {args.SocketError}");
            }
        }
    }
}
