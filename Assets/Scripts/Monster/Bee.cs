using UnityEngine;

public class Bee : Character
{
    protected new void Awake()
    {
        base.Awake();
    }

    protected new void OnEnable()
    {
        base.OnEnable();
    }

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
    }

    protected new void OnDisable()
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

    protected override void OnChildHitEvent() {}
}
