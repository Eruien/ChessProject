using System;
using ServerCore;
using UnityEngine;

namespace Assets.Scripts
{
    public class PacketHandler
    {
        static PacketHandler m_PacketHandler = new PacketHandler();
        public static PacketHandler Instance { get { return m_PacketHandler; } }

        private GameObject skeletonPrefab;

        public void PurchaseAllowedPacket(Session session, IPacket packet)
        {
            PurchaseAllowedPacket purchaseAllowed = packet as PurchaseAllowedPacket;
            if (purchaseAllowed.IsPurchase)
            {
                Debug.Log("샀어요");
                Managers.Resource.Instantiate("Skeleton", new Vector3(0.0f, 1.0f, 0.0f));
            }
            else
            {
                Debug.Log("돈이 없어서 못사요");
            }
        }

        public void MovePacketHandler(Session session, IPacket packet)
        {
            Console.WriteLine("Move Pacekt Handler 작동");
        }

        public void PlayerListHandler(Session session, IPacket packet)
        {
            Console.WriteLine("클라이언트에 플레이어 목록 등록");
            //Program.g_ClientPlayerList = packet as PlayerList;
        }

        public void S_BroadcastEnterGameHandler(Session session, IPacket packet)
        {
            S_BroadcastEnterGame enterPlayer = packet as S_BroadcastEnterGame;
           /* Program.g_ClientPlayerList.Add(new PlayerList.Player
            {
                PlayerId = enterPlayer.playerId,
                PosX = enterPlayer.posX,
                PosY = enterPlayer.posY,
                PosZ = enterPlayer.posZ,
            });*/
        }
    }
}
