﻿using ServerCore;
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
            Global.g_MyTeam = (Team)setDataPacket.m_MyTeam;
            Global.g_UserMoney = setDataPacket.m_InitialUserMoney;
            UserMoney.changeUserMoneyText.Invoke();
            Camera.main.gameObject.GetComponent<CameraController>().SetTeamCameraTransform(Global.g_MyTeam);
            
            Debug.Log($"지금 팀은 {Global.g_MyTeam}");
        }

        public void S_LabListPacketHandler(Session session, IPacket packet)
        {
            S_LabListPacket labPacket = packet as S_LabListPacket;

            for (int i = 0; i < labPacket.m_LabList.Count; i++)
            {
                GameObject obj = null;

                if (labPacket.m_LabList[i].m_Team == (ushort)Team.RedTeam)
                {
                    obj = Managers.Resource.Instantiate("RedTeamLab", new Vector3(labPacket.m_LabList[i].m_PosX, labPacket.m_LabList[i].m_PosY, labPacket.m_LabList[i].m_PosZ));
                }
                else if (labPacket.m_LabList[i].m_Team == (ushort)Team.BlueTeam)
                {
                    obj = Managers.Resource.Instantiate("BlueTeamLab", new Vector3(labPacket.m_LabList[i].m_PosX, labPacket.m_LabList[i].m_PosY, labPacket.m_LabList[i].m_PosZ));
                }

                Managers.Monster.Register(labPacket.m_LabList[i].m_LabId, obj);
                obj.GetComponent<BaseObject>().ObjectId = labPacket.m_LabList[i].m_LabId;
            }
        }

        public void S_BroadcastGameStartPacketHandler(Session session, IPacket packet)
        {
            S_BroadcastGameStartPacket gameStart = packet as S_BroadcastGameStartPacket;

            if (gameStart.m_IsGameStart)
            {
                GameStartEvent.GameStart.Invoke();
            }
        }

        public void S_PurchaseAllowedPacketHandler(Session session, IPacket packet)
        {
            S_PurchaseAllowedPacket purchaseAllowed = packet as S_PurchaseAllowedPacket;
            Global.g_UserMoney = purchaseAllowed.m_UserGameMoney;
            UserMoney.changeUserMoneyText.Invoke();
            C_MonsterCreatePacket monsterCreatePacket = new C_MonsterCreatePacket();
            monsterCreatePacket.m_StringSize = purchaseAllowed.m_StringSize;
            monsterCreatePacket.m_MonsterType = purchaseAllowed.m_MonsterType;

            if (purchaseAllowed.m_IsPurchase)
            {
                Debug.Log("샀어요");
       
                monsterCreatePacket.m_PosX = purchaseAllowed.m_PosX;
                monsterCreatePacket.m_PosY = purchaseAllowed.m_PosY;
                monsterCreatePacket.m_PosZ = purchaseAllowed.m_PosZ;
                SessionManager.Instance.GetServerSession().Send(monsterCreatePacket.Write());
            }
            else
            {
                Debug.Log("돈이 없어서 못사요");
            }
        }

        public void S_BroadcastMonsterCreatePacketHandler(Session session, IPacket packet)
        {
            S_BroadcastMonsterCreatePacket monsterPacket = packet as S_BroadcastMonsterCreatePacket;
            GameObject obj = Managers.Resource.Instantiate(monsterPacket.m_MonsterType, new Vector3(monsterPacket.m_PosX, monsterPacket.m_PosY, monsterPacket.m_PosZ));

            if (monsterPacket.m_MonsterTeam == (ushort)Team.BlueTeam)
            {
                obj.transform.eulerAngles = new Vector3(0, -180, 0);
            }

            obj.layer = monsterPacket.m_MonsterTeam;
            Managers.Monster.Register(monsterPacket.m_MonsterId, obj);
            Managers.Spawn.RegisterPanel(Managers.Spawn.SearchPanelGameObject(obj, "SpawnPlane"));
            obj.GetComponent<BaseMonster>().ObjectId = monsterPacket.m_MonsterId;
            obj.GetComponent<BaseMonster>().TargetLab = Managers.Monster.GetMonster(monsterPacket.m_TargetLabId);
            obj.GetComponent<BaseMonster>().Target = obj.GetComponent<BaseMonster>().TargetLab;
        }

        public void S_BroadcastMonsterStatePacketHandler(Session session, IPacket packet)
        {
            S_BroadcastMonsterStatePacket monsterStatePacket = packet as S_BroadcastMonsterStatePacket;
            GameObject obj = Managers.Monster.GetMonster(monsterStatePacket.m_MonsterId);

            if (obj != null)
            {
                Vector3 serverPos = new Vector3(monsterStatePacket.m_PosX, monsterStatePacket.m_PosY, monsterStatePacket.m_PosZ);
               
                if (obj.GetComponent<BaseMonster>().MonsterState != MonsterState.Attack &&
                    (MonsterState)monsterStatePacket.m_CurrentState == MonsterState.Attack)
                {
                    if (Vector3.Distance(obj.transform.position, serverPos) >= 0.1f)
                    {
                        if (obj.layer != (int)Global.g_MyTeam) return;
                        C_ConfirmMovePacket confirmMovePacket = new C_ConfirmMovePacket();
                        confirmMovePacket.m_MonsterId = (ushort)obj.GetComponent<BaseObject>().ObjectId;
                        SessionManager.Instance.GetServerSession().Send(confirmMovePacket.Write());
                        return;
                    }
                }
                obj.GetComponent<BaseMonster>().MonsterState = (MonsterState)monsterStatePacket.m_CurrentState;
            }
        }

        public void S_BroadcastMonsterDeathPacketHandler(Session session, IPacket packet)
        {
            S_BroadcastMonsterDeathPacket monsterDeathPacket = packet as S_BroadcastMonsterDeathPacket;
            GameObject obj = Managers.Monster.GetMonster(monsterDeathPacket.m_MonsterId);

            if (obj != null)
            {
                obj.GetComponent<BaseObject>().Death();
            }
        }

        public void S_BroadcastSetPositionPacketHandler(Session session, IPacket packet)
        {
            S_BroadcastSetPositionPacket positionPacket = packet as S_BroadcastSetPositionPacket;
            GameObject obj = Managers.Monster.GetMonster(positionPacket.m_MonsterId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().transform.position = new Vector3(positionPacket.m_PosX, positionPacket.m_PosY, positionPacket.m_PosZ);
            }
        }

        public void S_BroadcastMovePacketHandler(Session session, IPacket packet)
        {
            S_BroadcastMovePacket movePacket = packet as S_BroadcastMovePacket;
            GameObject obj = Managers.Monster.GetMonster(movePacket.m_MonsterId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().MovePos = new Vector3(movePacket.m_PosX, movePacket.m_PosY, movePacket.m_PosZ);
            }
        }

        public void S_BroadcastHitPacketHandler(Session session, IPacket packet)
        {
            S_BroadcastHitPacket hitPacket = packet as S_BroadcastHitPacket;
            
            GameObject obj = Managers.Monster.GetMonster(hitPacket.m_TargetId);

           /* if (obj != null)
            {
                obj.GetComponent<BaseObject>().blackBoard.m_HP.Key = hitPacket.m_TargetHP;
                if (obj.GetComponent<BaseObject>().blackBoard.m_HP.Key <= 0)
                {
                    Managers.Monster.UnRegister(obj);
                }
            }*/
        }

        public void S_BroadcastChangeTargetPacketHandler(Session session, IPacket packet)
        {
            S_BroadcastChangeTargetPacket changeTargetPacket = packet as S_BroadcastChangeTargetPacket;

            GameObject obj = Managers.Monster.GetMonster(changeTargetPacket.m_ObjectId);

            if (obj != null)
            {
                obj.GetComponent<BaseMonster>().Target = Managers.Monster.GetMonster(changeTargetPacket.m_TargetObjectId);
            }
        }
    }
}
