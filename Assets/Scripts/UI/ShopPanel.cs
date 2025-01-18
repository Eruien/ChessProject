using Assets.Scripts;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    private int gameMoney = 4000;

    public void OnCreateSkeleton()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Skeleton";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = gameMoney;
        purchasePacket.m_MonsterPrice = 500;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnGameStart()
    {
        C_GameStartPacket gameStartPacket = new C_GameStartPacket();
        gameStartPacket.m_IsGameStart = true;
        
        SessionManager.Instance.GetServerSession().Send(gameStartPacket.Write());
    }
}
