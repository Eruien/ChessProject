using Assets.Scripts;
using UnityEngine;

public class Labo : BaseObject
{
    protected override void SetBlackBoardKey()
    {
        blackBoard.m_HP.Key = 100.0f;
    }

    private void Awake()
    {
        
    }
}
