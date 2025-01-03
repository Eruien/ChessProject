using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Labo : BaseObject
{
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
        IsHPZero();
    }

    public void TransportLaboData()
    {
        C_SetInitialLaboPacket laboPacket = new C_SetInitialLaboPacket();
        laboPacket.laboTeam = (ushort)gameObject.layer;
        laboPacket.laboPosX = gameObject.transform.position.x;
        laboPacket.laboPosY = gameObject.transform.position.y;
        laboPacket.laboPosZ = gameObject.transform.position.z;
        SessionManager.Instance.GetServerSession().Send(laboPacket.Write());
    }

    private void IsHPZero()
    {
        if (blackBoard.m_HP.Key <= 0)
        {
            Destroy(gameObject);
        }
    }
}
