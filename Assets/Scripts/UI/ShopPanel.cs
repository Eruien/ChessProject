using Assets.Scripts;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    private int gameMoney = 4000;

    public void OnCreateSkeleton()
    {
        C_MonsterPurchasePacket purchasePacket = new C_MonsterPurchasePacket();
        string monsterType = "Mage";
        purchasePacket.m_StringSize = (ushort)monsterType.Length;
        purchasePacket.m_MonsterType = monsterType;
        purchasePacket.m_UserGameMoney = gameMoney;
        purchasePacket.m_MonsterPrice = 500;
        purchasePacket.m_PosX = 0.0f;
        purchasePacket.m_PosY = 1.0f;
        purchasePacket.m_PosZ = 0.0f;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnGameStart()
    {
        C_GameStartPacket gameStartPacket = new C_GameStartPacket();
        gameStartPacket.m_IsGameStart = true;
        
        SessionManager.Instance.GetServerSession().Send(gameStartPacket.Write());
    }
}
