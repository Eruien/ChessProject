
public class Golem : BaseMonster
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
        MonsterType = Managers.Data.monsterDict[this.GetType().Name].monsterType;
        blackBoard.m_HP.Key = Managers.Data.monsterDict[this.GetType().Name].hp;
        blackBoard.m_AttackDistance.Key = Managers.Data.monsterDict[this.GetType().Name].attackDistance;
        blackBoard.m_AttackRange.Key = Managers.Data.monsterDict[this.GetType().Name].attackRange;
        blackBoard.m_AttackRangeCorrectionValue.Key = Managers.Data.monsterDict[this.GetType().Name].attackRangeCorrectionValue;
        blackBoard.m_DefaultAttackDamage.Key = Managers.Data.monsterDict[this.GetType().Name].defaultAttackDamage;
        blackBoard.m_MoveSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].moveSpeed;
        blackBoard.m_ProjectTileSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].projectTileSpeed;
    }

    protected override void ChildAttack() { }

    protected override void OnChildHitEvent() { }
}
