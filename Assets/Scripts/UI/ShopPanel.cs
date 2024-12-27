using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    private int gameMoney = 4000;

    public void OnCreateSkeleton()
    {
        MonsterPurchasePacket purchasePacket = new MonsterPurchasePacket();
        purchasePacket.userGameMoney = gameMoney;
        purchasePacket.monsterPrice = 500;
        purchasePacket.PosX = 0.0f;
        purchasePacket.PosY = 1.0f;
        purchasePacket.PosZ = 0.0f;

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }

    public void OnGameStart()
    {
        GameStartPacket gameStartPacket = new GameStartPacket();
        gameStartPacket.IsGameStart = true;
        
        SessionManager.Instance.GetServerSession().Send(gameStartPacket.Write());
    }
}
