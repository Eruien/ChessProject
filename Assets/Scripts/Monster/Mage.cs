using UnityEngine;

public class Mage : Character
{
    private GameObject fireBallPrefab;
    private GameObject fireBall;
    private GameObject weaponSocket;
   
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
        blackBoard.m_HP.Key = Managers.Data.monsterDict[this.GetType().Name].hp;
        blackBoard.m_AttackRange.Key = Managers.Data.monsterDict[this.GetType().Name].attackRange;
        blackBoard.m_AttackRangeCorrectionValue.Key = Managers.Data.monsterDict[this.GetType().Name].attackRangeCorrectionValue;
        blackBoard.m_AttackDistance.Key = Managers.Data.monsterDict[this.GetType().Name].attackDistance;
        target = targetLabo;
        blackBoard.m_TargetObject.Key = target;
    }

    protected override void ChildAttack() {}

    protected override void OnChildHitEvent()
    {
        if (fireBall == null) return;
        fireBall.GetComponent<FireBall>().IsUse = false;
    }

    // AttackAnimationCheck에서 OnAttackProjectTile 이벤트 용
    public void OnAttackFireBall()
    {
        fireBall = Instantiate(fireBallPrefab, weaponSocket.transform.position, Quaternion.identity);
        FireBall fireBallScript = fireBall.GetComponent<FireBall>();
        fireBallScript.TargetObject = blackBoard.m_TargetObject;
        fireBallScript.AttackRange = blackBoard.m_AttackRange.Key;
        fireBallScript.AttackRangeCorrectionValue = blackBoard.m_AttackRangeCorrectionValue.Key;
        fireBall.GetComponent<CollisionCheck>().CollisionAddListener(OnHitEvent);
        fireBall.GetComponent<SelectColliderExclude>().SelectExcludeLayer(gameObject.layer);
    }
}
