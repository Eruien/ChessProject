using Assets.Scripts;
using UnityEngine;

public class Labo : BaseObject
{
    [SerializeField]
    public float viewHP = 0.0f;

    protected override void SetBlackBoardKey()
    {
        blackBoard.m_HP.Key = 100.0f;
    }

    public override void SetPosition(float x, float y, float z)
    {
        transform.position = new UnityEngine.Vector3(x, y, z);
    }

    private void Awake()
    {
        SetBlackBoardKey();
        SelfType = ObjectType.Machine;
    }

    private void Update()
    {
        viewHP = blackBoard.m_HP.Key;

        IsHPZero();
    }

    private void IsHPZero()
    {
        if (blackBoard.m_HP.Key <= 0)
        {
            Destroy(gameObject);
        }
    }
}
