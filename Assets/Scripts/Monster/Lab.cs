using Assets.Scripts;

public class Lab : BaseObject
{
    private void Awake()
    {
        SetBlackBoardKey();
        SelfType = ObjectType.Machine;
    }

    public override void SetPosition(float x, float y, float z)
    {
        transform.position = new UnityEngine.Vector3(x, y, z);
    }

    public override void Death()
    {
        Managers.Monster.UnRegister(gameObject);
    }

    protected override void SetBlackBoardKey()
    {
        blackBoard.m_HP.Key = 100.0f;
    }
}
