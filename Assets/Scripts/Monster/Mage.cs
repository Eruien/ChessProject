using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{
    GameObject fireBallPrefab;
    GameObject fireBall;
    GameObject weaponSocket;
   
    protected new void Awake()
    {
        base.Awake();
        fireBallPrefab = Managers.Resource.Load<GameObject>("Prefabs/FireBall");
        weaponSocket = FindChildObject("Socket");
    }

    private new void OnEnable()
    {
        base.OnEnable();
    }

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }

    protected override void SetBlackBoardKey()
    {
        Debug.Log(this.name);
        blackBoard.m_HP.Key = Managers.Data.monsterDict[this.GetType().Name].hp;
        blackBoard.m_AttackRange.Key = Managers.Data.monsterDict[this.GetType().Name].attackRange;
        blackBoard.m_AttackRangeCorrectionValue.Key = Managers.Data.monsterDict[this.GetType().Name].attackRangeCorrectionValue;
        blackBoard.m_AttackDistance.Key = Managers.Data.monsterDict[this.GetType().Name].attackDistance;
        target = targetLabo;
        blackBoard.m_TargetObject.Key = target;
    }

    protected override void ChildAttack() {}

    // AttackAnimationCheck���� OnAttackProjectTile �̺�Ʈ ��
    public void OnAttackFireBall()
    {
        fireBall = Instantiate(fireBallPrefab, weaponSocket.transform.position, Quaternion.identity);
        fireBall.GetComponent<FireBall>().TargetObject = blackBoard.m_TargetObject;
        fireBall.GetComponent<FireBall>().AttackRange = blackBoard.m_AttackRange.Key;
        fireBall.GetComponent<FireBall>().AttackRangeCorrectionValue = blackBoard.m_AttackRangeCorrectionValue.Key;
        fireBall.GetComponent<CollisionCheck>().UnityHitEvent.AddListener(OnHitEvent);
        fireBall.GetComponent<SphereCollider>().excludeLayers = (1 << gameObject.layer) | fireBall.GetComponent<SphereCollider>().excludeLayers;
    }

    public override void OnChildHitEvent()
    {
        fireBall.GetComponent<CollisionCheck>().UnityHitEvent.RemoveListener(OnHitEvent);
        Destroy(fireBall.gameObject);
    }
}
