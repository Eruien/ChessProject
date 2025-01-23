using System;
using System.Collections.Generic;
using ServerCore;

namespace Assets.Scripts
{
    public class PacketManager
    {
        static PacketManager m_PacketMgr = new PacketManager();
        public static PacketManager Instance { get { return m_PacketMgr; } }

        Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> m_MakePacketDict = new Dictionary<ushort, Func<ArraySegment<byte>, IPacket>>();
        Dictionary<ushort, Action<Session, IPacket>> m_RunFunctionDict = new Dictionary<ushort, Action<Session, IPacket>>();

        public PacketManager()
        {
            Init();
        }

        void Init()
        {
            m_MakePacketDict.Add((ushort)PacketType.S_SetInitialDataPacket, MakePacket<S_SetInitialDataPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_SetInitialDataPacket, PacketHandler.Instance.S_SetInitialDataPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_LabListPacket, MakePacket<S_LabListPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_LabListPacket, PacketHandler.Instance.S_LabListPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastGameStartPacket, MakePacket<S_BroadcastGameStartPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastGameStartPacket, PacketHandler.Instance.S_BroadcastGameStartPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_PurchaseAllowedPacket, MakePacket<S_PurchaseAllowedPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_PurchaseAllowedPacket, PacketHandler.Instance.S_PurchaseAllowedPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastMonsterCreatePacket, MakePacket<S_BroadcastMonsterCreatePacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastMonsterCreatePacket, PacketHandler.Instance.S_BroadcastMonsterCreatePacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastMonsterStatePacket, MakePacket<S_BroadcastMonsterStatePacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastMonsterStatePacket, PacketHandler.Instance.S_BroadcastMonsterStatePacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastMonsterDeathPacket, MakePacket<S_BroadcastMonsterDeathPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastMonsterDeathPacket, PacketHandler.Instance.S_BroadcastMonsterDeathPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastMovePacket, MakePacket<S_BroadcastMovePacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastMovePacket, PacketHandler.Instance.S_BroadcastMovePacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastHitPacket, MakePacket<S_BroadcastHitPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastHitPacket, PacketHandler.Instance.S_BroadcastHitPacketHandler);

            m_MakePacketDict.Add((ushort)PacketType.S_BroadcastChangeTargetPacket, MakePacket<S_BroadcastChangeTargetPacket>);
            m_RunFunctionDict.Add((ushort)PacketType.S_BroadcastChangeTargetPacket, PacketHandler.Instance.S_BroadcastChangeTargetPacketHandler);
        }

        public int OnRecvPacket(Session session, ArraySegment<byte> buffer, Action<Session, IPacket> onRecvCallback = null)
        {
            // packet 번호에 따라 패킷 생성
            // 패킷 번호에 따라 맞는 함수 실행
            int count = 0;
            count += sizeof(ushort);
            ushort packetID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);

            Func<ArraySegment<byte>, IPacket> func = null;

            if (m_MakePacketDict.TryGetValue(packetID, out func))
            {
                IPacket packet = func.Invoke(buffer);

                if (onRecvCallback != null)
                    onRecvCallback.Invoke(session, packet);
                else
                    HandlePacket(packetID, session, packet);

                return packet.PacketSize;
            }

            return 0;
        }

        T MakePacket<T>(ArraySegment<byte> buffer) where T: IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);

            return pkt; 
        }

        public void HandlePacket(ushort packetID, Session session, IPacket packet)
        {
            Action<Session, IPacket> action = null;

            if (m_RunFunctionDict.TryGetValue(packetID, out action))
            {
                action.Invoke(session, packet);
            }
        }
    }
}
