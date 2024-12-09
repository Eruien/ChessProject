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
        fireBallPrefab = Managers.Resource.Load<GameObject>("Prefab/FireBall");
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
        /*Vector3 direction = (target.transform.position - instance.transform.position).normalized;
        instance.transform.position += direction * homingSpeed * Time.deltaTime;*/
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }

    protected override void SetBlackBoardKey()
    {
        DataManager.Instance.FetchData();
        blackBoard.m_HP.Key = 100.0f;
        blackBoard.m_SearchRange.Key = 20.0f;
        blackBoard.m_AttackDistance.Key = ComputeAttackDistance();
        blackBoard.m_AttackRange.Key = 10.0f;
        target = targetLabo;
        blackBoard.m_TargetObject.Key = target;
    }

    protected override void ChildAttack()
    {
        fireBall = Instantiate(fireBallPrefab, weaponSocket.transform.position, Quaternion.identity);
        fireBall.GetComponent<FireBall>().TargetObject = blackBoard.m_TargetObject;
    }



}
