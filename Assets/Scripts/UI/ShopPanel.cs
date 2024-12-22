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

        SessionManager.Instance.GetServerSession().Send(purchasePacket.Write());
    }
}
