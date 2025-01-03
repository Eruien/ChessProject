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

        public void S_SetInitialDataPacketHandler(Session session, IPacket packet)
        {
            S_SetInitialDataPacket setDataPacket = packet as S_SetInitialDataPacket;
            Global.g_MyTeam = (Team)setDataPacket.myTeam;
            Debug.Log($"지금 팀은 {Global.g_MyTeam}");

            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Labo");

            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == (int)Global.g_MyTeam)
                {
                    obj.GetComponent<Labo>().TransportLaboData();
                }
            }
        }

        public void S_SetInitialLaboPacketHandler(Session session, IPacket packet)
        {
            S_SetInitialLaboPacket laboPacket = packet as S_SetInitialLaboPacket;

            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Labo");

            foreach (GameObject obj in allObjects)
            {
                if (laboPacket.laboOneTeam == obj.layer)
                {
                    Managers.Monster.Register(laboPacket.laboOneId, obj);
                    obj.GetComponent<BaseObject>().ObjectId = laboPacket.laboOneId;
                }
                else if (laboPacket.laboTwoTeam == obj.layer)
                {
                    Managers.Monster.Register(laboPacket.laboTwoId, obj);
                    obj.GetComponent<BaseObject>().ObjectId = laboPacket.laboTwoId;
                }
            }
        }

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
            GameObject obj = Managers.Monster.GetMonster(movePacket.objectId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().MovePos = new Vector3(movePacket.PosX, movePacket.PosY, movePacket.PosZ);
            }
        }

        public void S_MonsterStatePacketHandler(Session session, IPacket packet)
        {
            S_MonsterStatePacket monsterStatePacket = packet as S_MonsterStatePacket;
            GameObject obj = Managers.Monster.GetMonster(monsterStatePacket.objectId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().MonsterState = (MonsterState)monsterStatePacket.currentState;
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
            obj.layer = monsterPacket.monsterTeam;
            Managers.Monster.Register(monsterPacket.objectId, obj);
            obj.GetComponent<BaseMonster>().SetPosition(monsterPacket.PosX, monsterPacket.PosY, monsterPacket.PosZ);
            obj.GetComponent<BaseMonster>().ObjectId = monsterPacket.objectId;
        }

        public void S_HitPacketHandler(Session session, IPacket packet)
        {
            S_HitPacket hitPacket = packet as S_HitPacket;
            
            GameObject obj = Managers.Monster.GetMonster(hitPacket.objectId);

            if (obj != null)
            {
                obj.GetComponent<BaseObject>().blackBoard.m_HP.Key = hitPacket.objectHP;
            }
        }

        public void S_ChangeTargetPacketHandler(Session session, IPacket packet)
        {
            S_ChangeTargetPacket changeTargetPacket = packet as S_ChangeTargetPacket;

            GameObject obj = Managers.Monster.GetMonster(changeTargetPacket.objectId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().Target = Managers.Monster.GetMonster(changeTargetPacket.targetObjectId);
            }
        }
    }
}
