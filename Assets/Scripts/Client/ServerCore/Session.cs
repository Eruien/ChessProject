﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;

namespace ServerCore
{
    abstract public class PacketSession : Session
    {
        public override int OnRecv(ArraySegment<byte> buffer)
        {
            int count = 0;
            // 큰 덩어리로 들어왔을 경우 쪼개서 읽기
            // 데이터가 작을 경우 그냥 패스
           
            while (true)
            {
                // 헤더 사이즈 보다 작다면 패스
                if (buffer.Count < Global.g_HeaderSize) break;

                int dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);

                if (buffer.Count > dataSize) break;

                // 이제 데이터 읽기 가능
                count += OnRecvPacket(buffer);

                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + count, buffer.Count - count);
            }

            return count;
        }

        public abstract int OnRecvPacket(ArraySegment<byte> buffer);
    }

    abstract public class Session
    {
        Socket m_Socket;
        SocketAsyncEventArgs m_SendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs m_RecvArgs = new SocketAsyncEventArgs();
        RecvBuffer m_RecvBuffer = new RecvBuffer(65535);
        object m_Lock = new object();

        Queue<ArraySegment<byte>> m_SendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> m_PendingList = new List<ArraySegment<byte>>();
        List<ArraySegment<byte>> m_RecvBufferList = new List<ArraySegment<byte>>();

        public abstract void OnConnect();
        public abstract void OnSend(int numOfBytes);
        public abstract int OnRecv(ArraySegment<byte> buffer);
        public abstract void OnDisconnect();

        // 소켓 등록, 이벤트 등록
        public void Init(Socket socket)
        {
            m_Socket = socket;
            m_SendArgs.Completed += OnSendCompleted;
            m_RecvArgs.Completed += OnRecvCompleted;
        }

        public void Start()
        {
            RegisterRecv();
        }

        public void Send(List<ArraySegment<byte>> segmentList)
        {
            if (segmentList.Count == 0) return;

            lock (m_Lock)
            {
                foreach (var arg in segmentList)
                    m_SendQueue.Enqueue(arg);

                if (m_PendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Send(ArraySegment<byte> segment)
        {
            lock (m_Lock)
            {
                m_SendQueue.Enqueue(segment);

                if (m_PendingList.Count == 0)
                    RegisterSend();
            }
        }

        // 다른곳에서 Send했을 때 모아서 쏘는 역할
        void RegisterSend()
        {
            while (m_SendQueue.Count > 0)
            {
                m_PendingList.Add(m_SendQueue.Dequeue());
            }

            m_SendArgs.BufferList = m_PendingList;

            try
            {
                bool pending = m_Socket.SendAsync(m_SendArgs);

                if (pending == false)
                    OnSendCompleted(null, m_SendArgs);
            }
            catch ( Exception e)
            {
                Console.WriteLine($"Session -> SendAsync Error : {e}");
            }
            
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (m_Lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        Console.WriteLine("메시지 전송 성공");
                        m_PendingList.Clear();
                        m_SendArgs.BufferList = null;

                        OnSend(m_SendArgs.BytesTransferred);

                        if (m_SendQueue.Count > 0)
                            RegisterSend();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Session -> OnSendCompletedError : {e}");
                    }
                }
                else
                {
                    DisConnect();
                }
            }
        }

        void RegisterRecv()
        {
            // 초기화, 데이터를 받고 나서 다시 작동하는거라서 상관없을거임
            m_RecvArgs.BufferList = null;
            Clear();

            ArraySegment<byte> segment = m_RecvBuffer.WriteSegemnt;
           
            m_RecvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);
            bool pending = m_Socket.ReceiveAsync(m_RecvArgs);
            if (pending == false)
                OnRecvCompleted(null, m_RecvArgs);
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            UnityEngine.Debug.Log("메시지에 문제가 있어요");
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                UnityEngine.Debug.Log("서버로 부터 메시지를 받았어요");
                m_RecvBuffer.OnWrite(args.BytesTransferred);
                // 읽는건 컨텐츠 딴에 맡김
                int readSize = OnRecv(m_RecvBuffer.ReadSegemnt);

                m_RecvBuffer.OnRead(readSize);

                if (readSize < args.BytesTransferred)
                {
                    Console.WriteLine("Session -> OnRecvCompleted : 전송받은것보다 적게 읽어냈습니다. ");
                    return;
                }
                RegisterRecv();
            }
            else
            {
                DisConnect();
            }
        }

        void DisConnect()
        {
            m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close();
            Clear();
        }

        void Clear()
        {
            lock (m_Lock)
            {
                m_SendQueue.Clear();
                m_PendingList.Clear();
            }
        }
    }
}