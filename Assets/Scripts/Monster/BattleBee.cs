using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBee : BaseMonster
{
    private GameObject projectTilePrefab;
    private GameObject projectTile;
    private GameObject weaponSocket;

    protected new void Awake()
    {
        base.Awake();
        projectTilePrefab = Managers.Resource.Load<GameObject>("Prefabs/FireBall");
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
        target = targetLabo;
        blackBoard.m_TargetObject.Key = target;
        blackBoard.m_HP.Key = Managers.Data.monsterDict[this.GetType().Name].hp;
        blackBoard.m_AttackDistance.Key = Managers.Data.monsterDict[this.GetType().Name].attackDistance;
        blackBoard.m_AttackRange.Key = Managers.Data.monsterDict[this.GetType().Name].attackRange;
        blackBoard.m_AttackRangeCorrectionValue.Key = Managers.Data.monsterDict[this.GetType().Name].attackRangeCorrectionValue;
        blackBoard.m_DefaultAttackDamage.Key = Managers.Data.monsterDict[this.GetType().Name].defaultAttackDamage;
        blackBoard.m_MoveSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].moveSpeed;
        blackBoard.m_ProjectTileSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].projectTileSpeed;
    }

    protected override void ChildAttack() { }

    protected override void OnChildHitEvent()
    {
        if (projectTile == null) return;
        projectTile.GetComponent<ProjectTile>().IsUse = false;
    }

    // AttackAnimationCheck에서 OnAttackProjectTile 이벤트 용
    public void OnAttackMonsterProjectTile()
    {
        projectTile = Instantiate(projectTilePrefab, weaponSocket.transform.position, Quaternion.identity);
        ProjectTile projectTileScript = projectTile.GetComponent<ProjectTile>();
        projectTileScript.TargetObject = blackBoard.m_TargetObject;
        projectTileScript.AttackRange = blackBoard.m_AttackRange.Key;
        projectTileScript.AttackRangeCorrectionValue = blackBoard.m_AttackRangeCorrectionValue.Key;
        projectTileScript.ProjectTileSpeed = blackBoard.m_ProjectTileSpeed.Key;
        projectTile.GetComponent<CollisionCheck>().CollisionAddListener(OnHitEvent);
        projectTile.GetComponent<SelectColliderExclude>().SelectExcludeLayer(gameObject.layer);
    }
}
