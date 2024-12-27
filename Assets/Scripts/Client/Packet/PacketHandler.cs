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
                GameObject obj = Managers.Resource.Instantiate("Skeleton", new Vector3(0.0f, 1.0f, 0.0f));
                int id = Managers.Monster.Register(obj);
                C_MonsterCreatePacket monsterCreatePacket = new C_MonsterCreatePacket();
                monsterCreatePacket.monsterId = (ushort)id;
                monsterCreatePacket.monsterTeam = 1;
                monsterCreatePacket.PosX = obj.transform.position.x;
                monsterCreatePacket.PosY = obj.transform.position.y;
                monsterCreatePacket.PosZ = obj.transform.position.z;
                SessionManager.Instance.GetServerSession().Send(monsterCreatePacket.Write());
            }
            else
            {
                Debug.Log("돈이 없어서 못사요");
            }
        }

        public void MovePacketHandler(Session session, IPacket packet)
        {
            MovePacket movePacket = packet as MovePacket;
            GameObject obj = Managers.Monster.GetMonster(movePacket.monsterId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().MovePos = new Vector3(movePacket.PosX, movePacket.PosY, movePacket.PosZ);
            }
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
