using System;
using ServerCore;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class PacketHandler
    {
        static PacketHandler m_PacketHandler = new PacketHandler();
        public static PacketHandler Instance { get { return m_PacketHandler; } }

        public void PurchaseAllowedPacket(Session session, IPacket packet)
        {
            PurchaseAllowedPacket purchaseAllowed = packet as PurchaseAllowedPacket;

            if (purchaseAllowed.IsPurchase)
            {
                Debug.Log("샀어요");
          
                C_MonsterCreatePacket monsterCreatePacket = new C_MonsterCreatePacket();
              
                monsterCreatePacket.monsterTeam = 1;
                monsterCreatePacket.PosX = purchaseAllowed.PosX;
                monsterCreatePacket.PosY = purchaseAllowed.PosY;
                monsterCreatePacket.PosZ = purchaseAllowed.PosZ;
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

        public void S_BroadcastMonsterCreatePacketHandler(Session session, IPacket packet)
        {
            S_BroadcastMonsterCreatePacket monsterPacket = packet as S_BroadcastMonsterCreatePacket;
            GameObject obj = Managers.Resource.Instantiate("Skeleton", new Vector3(0.0f, 1.0f, 0.0f));
            Managers.Monster.Register(monsterPacket.monsterId, obj);
            obj.GetComponent<BaseMonster>().SetPosition(monsterPacket.PosX, monsterPacket.PosY, monsterPacket.PosZ);
        }
    }
}
