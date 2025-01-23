using Assets.Scripts;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public void OnCreateSkeleton()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Skeleton";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateMage()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Mage";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateGolem()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Golem";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateSpecter()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Specter";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateBee()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Bee";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateBattleBee()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "BattleBee";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
        Vector3 spawnPoint = Managers.Spawn.ComputeSpawnPoint(Global.g_MyTeam, monsterType);
        purchasePacket.m_PosX = spawnPoint.x;
        purchasePacket.m_PosY = spawnPoint.y;
        purchasePacket.m_PosZ = spawnPoint.z;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnCreateBeholder()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Beholder";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = (ushort)Global.g_UserMoney;
        purchasePacket.m_MonsterPrice = (ushort)Managers.Data.monsterDict[monsterType].monsterPrice;
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
