using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BaseMonster : BaseObject
{
    [SerializeField]
    public float viewHP = 0.0f;
    [SerializeField]
    public bool IsNotDeath = false;
    [SerializeField]
    public bool fixPos = false;

    static public UnityEvent monsterDeathEvent = new UnityEvent();

    public GameObject Target { get; set; }
    public GameObject TargetLabo { get; set; }

    public Vector3 InitialPos { get; set; } = Vector3.zero;
    public Vector3 MovePos { get; set; } = Vector3.zero;
    public MonsterType MonsterType { get; set; } = MonsterType.None;
    public MonsterState MonsterState { get; set; } = MonsterState.None;
    public int MonsterId { get; set; } = 0;
  
    private Material material;
    private Animator monsterAnimation;
    private CapsuleCollider selfCollider;
    private Vector3 fixRotation = Vector3.zero;

    private bool UseLookAt = true;
    private bool IsHit = false;
    private bool attackAnimationSpan = false;
    private float monsterAlpha = 1.0f;
    private float initialY = 0.0f;
    private float deathAndDestroyTime = 1.0f;
    private float transportPacketTime = 0.1f;
    private float currentTime = 0.0f;

    // 유니티 라이프 사이클 함수 
    protected void Awake()
    {
        MonsterState = MonsterState.Move;
        SelfType = ObjectType.Monster;
        material = FindMaterial();
        monsterAnimation = GetComponentInChildren<Animator>();
        selfCollider = GetComponent<CapsuleCollider>();
        TargetLabo = FindTeamObjectWithTag("Labo");
        Target = TargetLabo;
        initialY = transform.position.y;
        InitialPos = transform.position;
        SetBlackBoardKey();
    }

    protected void OnEnable()
    {
        BaseMonster.monsterDeathEvent.AddListener(OnDeathEvent);
    }

    protected void Start()
    {
       
    }

    protected void Update()
    {
        viewHP = blackBoard.m_HP.Key;

        if (fixPos)
        {
            transform.position = InitialPos;
        }

        if (IsNotDeath)
        {
            blackBoard.m_HP.Key = 100;
        }

        monsterAnimation.SetBool("IsMove", false);

        if (!IsDeath)
        {
            currentTime += Time.deltaTime;

            ComputeAttackDistance();
            StateUpdate();

            if (currentTime >= transportPacketTime)
            {
                currentTime = 0.0f;
            }

            fixRotation = transform.eulerAngles;
            fixRotation.x = 0;
            fixRotation.z = 0;
            transform.eulerAngles = fixRotation;
        }
        else
        {
            StartCoroutine(DeathAndDestroy(deathAndDestroyTime));
        }
    }

    protected void OnDisable()
    {
        BaseMonster.monsterDeathEvent.RemoveListener(OnDeathEvent);
    }

    // 일반 함수

    private void TransportPacket(System.Action action)
    {
        if (gameObject.layer != (int)Global.g_MyTeam) return;

        if (currentTime >= transportPacketTime)
        {
            action.Invoke();
        }
    }

    private void ComputeAttackDistance()
    {
        if (Target == null) return;
        Vector3 vec = Target.transform.position - transform.position;
        float dis = Mathf.Pow(vec.x * vec.x + vec.z * vec.z, 0.5f);

        if (dis <= blackBoard.m_AttackDistance.Key)
        {
            C_AttackDistancePacket attackDistancePacket = new C_AttackDistancePacket();
            attackDistancePacket.monsterId = (ushort)MonsterId;
            attackDistancePacket.attackDistance = dis;
            TransportPacket(() => SessionManager.Instance.GetServerSession().Send(attackDistancePacket.Write()));
        }
    }

    public override void SetPosition(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    private Material FindMaterial()
    {
        Transform[] allObjects = transform.GetComponentsInChildren<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                Material mat = obj.gameObject.GetComponent<SkinnedMeshRenderer>().material;
                return mat;
            }
        }

        return null;
    }

    protected override void SetBlackBoardKey()
    {
        blackBoard.m_TargetObject.Key = Target;
        blackBoard.m_HP.Key = Managers.Data.monsterDict[this.GetType().Name].hp;
        blackBoard.m_AttackDistance.Key = Managers.Data.monsterDict[this.GetType().Name].attackDistance;
        blackBoard.m_AttackRange.Key = Managers.Data.monsterDict[this.GetType().Name].attackRange;
        blackBoard.m_AttackRangeCorrectionValue.Key = Managers.Data.monsterDict[this.GetType().Name].attackRangeCorrectionValue;
        blackBoard.m_DefaultAttackDamage.Key = Managers.Data.monsterDict[this.GetType().Name].defaultAttackDamage;
        blackBoard.m_MoveSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].moveSpeed;
        blackBoard.m_ProjectTileSpeed.Key = Managers.Data.monsterDict[this.GetType().Name].projectTileSpeed;
    }

    protected GameObject FindChildObject(string childName)
    {
        Transform[] allObjects = transform.GetComponentsInChildren<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.name == childName)
            {
                return obj.gameObject;
            }
        }

        return null;
    }

    protected GameObject FindTeamObjectWithTag(string tagName)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(tagName);

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer != gameObject.layer)
            {
                return obj.gameObject;
            }
        }

        return null;
    }

    // Coroutine
    private IEnumerator HPReduce(b_float characterHP)
    {
        while (characterHP.Key > 0)
        {
            characterHP.Key -= 1.0f;
            Debug.Log(characterHP.Key);
            yield return new WaitForSeconds(1.0f);
        }
    }
  
    private IEnumerator DecreaseAlpha()
    {
        while (monsterAlpha >= 0)
        {
            monsterAlpha -= 0.1f;
            material.SetFloat("_Alpha", monsterAlpha);  // 셰이더에 알파 값을 전달
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DeathAndDestroy(float deathTime)
    {
        yield return new WaitForSeconds(deathTime);
        Managers.Monster.UnRegister(gameObject);
    }

    // StatePattern 모음
    private void StateUpdate()
    {
        switch (MonsterState)
        {
            case MonsterState.Move:
                Move();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Death:
                Death();
                break;
            case MonsterState.None:
                Debug.Log("아무 상태도 없습니다.");
                break;
            default:
                Debug.Log("아무 상태도 없습니다.");
                break;
        }
    }

    private void Attack()
    {
        if (UseLookAt)
        {
            transform.LookAt(Target.transform);
            UseLookAt = false;
            monsterAnimation.SetTrigger("IsAttack");
            ChildAttack();
        }
    }

    // 자식에게서 추가 행동이 있을 경우
    protected virtual void ChildAttack() {}

    private void Move()
    {
        monsterAnimation.SetBool("IsMove", true);
        transform.LookAt(Target.transform);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, MovePos, blackBoard.m_MoveSpeed.Key * Time.deltaTime);
        Vector3 fixYPos = new Vector3(transform.position.x, initialY, transform.position.z);
        transform.position = fixYPos;
    }

    private void Death()
    {
        if (blackBoard.m_HP.Key <= 0)
        {
            monsterAnimation.SetBool("IsDeath", true);
            StartCoroutine(DecreaseAlpha());
            IsDeath = true;
            BaseMonster.monsterDeathEvent.Invoke();
        }
    }

    // 유니티 이벤트 모음
    // CollisionCheck의 이벤트 용
    public void OnHitEvent(Collider other)
    {
        if (other.gameObject == null) return;

        if (!IsHit && AttackType() && !IsDeath)
        {
            BaseObject otherObject = other.gameObject.GetComponent<BaseObject>();
            // HP 패킷 왔다갔다 
            IsHit = true;
            OnChildHitEvent();
        }
    }

    private bool AttackType()
    {
        if (MonsterType == MonsterType.Range)
        {
            return true;
        }

        return attackAnimationSpan;
    }

    // 자식에게서 추가 행동이 있을 경우
    protected virtual void OnChildHitEvent() {}

    // SearchCollision의 이벤트 용
    public void OnSearchCollisionEvent(Collider other)
    {
        if (Target != null)
        {
            BaseObject obj = Target.gameObject.GetComponent<BaseObject>();
            if (!obj.IsDeath && obj.SelfType == ObjectType.Monster) return;
        }

        Target = other.gameObject;
    }

    // 타격을 한 번만 입히게 하기 위해
    // AttackAnimationCheck의 이벤트 용
    public void OnAttackAnimationStart()
    {
        IsHit = false;
        attackAnimationSpan = true;
    }

    // 애니메이션 끝나는 타이밍 체크
    // AttackAnimationCheck의 이벤트 용
    public void OnAttackAnimationEnd()
    {
        UseLookAt = true;
        attackAnimationSpan = false;
    }

    // Character의 monsterDeathEvent 용
    public void OnDeathEvent()
    {
        if (!IsDeath)
        {
            selfCollider.enabled = false;
            selfCollider.enabled = true;
        }
    }
}
