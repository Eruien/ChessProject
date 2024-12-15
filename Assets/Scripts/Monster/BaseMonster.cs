using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseMonster : BaseObject
{
    [SerializeField]
    public GameObject target;
    [SerializeField]
    public GameObject targetLabo;

    [SerializeField]
    public float viewHP = 0.0f;
    [SerializeField]
    public bool IsNotDeath = false;

    static public UnityEvent monsterDeathEvent = new UnityEvent();
   
    private Material material;
    private Animator monsterAnimation;
    private CapsuleCollider selfCollider;
    private Selector selector;
    private Vector3 fixRotation = Vector3.zero;

    private bool UseLookAt = true;
    private bool IsHit = false;
    private bool attackAnimationSpan = false;
    private float monsterAlpha = 1.0f;
    private float initialY = 0.0f;
    private float deathAndDestroyTime = 1.0f;
   
    // 유니티 라이프 사이클 함수 
    protected void Awake()
    {
        SelfType = ObjectType.Monster;
        Managers.Monster.Register(gameObject);
        material = FindMaterial();
        monsterAnimation = GetComponentInChildren<Animator>();
        selfCollider = GetComponent<CapsuleCollider>();
        initialY = transform.position.y;
        targetLabo = GameObject.Find("Labo");
        selector = new Selector();

        SetBlackBoardKey();
    }

    protected void OnEnable()
    {
        BaseMonster.monsterDeathEvent.AddListener(OnDeathEvent);
    }

    protected void Start()
    {
        // HP관리
        Selector HPMgr = new Selector();
        selector.AddChild(HPMgr);

        Sequence<float, b_float> checkHPZero = new Sequence<float, b_float>(KeyQuery.IsLessThanOrEqualTo, 0.001f, blackBoard.m_HP);
        HPMgr.AddChild(checkHPZero);

        Action action = new Action(IsHPZero);
        checkHPZero.AddChild(action);

        // 공격 관리
        Selector attackMgr = new Selector();
        selector.AddChild(attackMgr);

        SetSequence<GameObject, b_GameObject> checkTargetLive = new SetSequence<GameObject, b_GameObject>(KeyQuery.IsSet, blackBoard.m_TargetObject);
        attackMgr.AddChild(checkTargetLive);

        Sequence<float, b_float> checkAttackRange = new Sequence<float, b_float>(KeyQuery.IsLessThanOrEqualTo, blackBoard.m_AttackRange.Key, blackBoard.m_AttackDistance);
        checkTargetLive.AddChild(checkAttackRange);

        Action attackAction = new Action(Attack);
        checkAttackRange.AddChild(attackAction);

        // 이동 관리
        Selector moveMgr = new Selector();
        selector.AddChild(moveMgr);

        SetSequence<GameObject, b_GameObject> move = new SetSequence<GameObject, b_GameObject>(KeyQuery.IsSet, blackBoard.m_TargetObject);
        moveMgr.AddChild(move);
        Action moveAction = new Action(MoveToPosition);

        move.AddChild(moveAction);
    }

    protected void Update()
    {
        viewHP = blackBoard.m_HP.Key;
     
        if (IsNotDeath)
        {
            blackBoard.m_HP.Key = 100;
        }

        monsterAnimation.SetBool("IsMove", false);

        if (!IsDeath)
        {
            if (target == null)
            {
                if (targetLabo != null)
                {
                    target = targetLabo;
                    blackBoard.m_TargetObject.Key = target;
                }
            }
            blackBoard.m_AttackDistance.Key = ComputeAttackDistance();
            selector.Tick();
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
    private float ComputeAttackDistance()
    {
        if (target == null) return blackBoard.m_AttackDistance.Key;
        Vector3 vec = target.transform.position - transform.position;
        float dis = Mathf.Pow(vec.x * vec.x + vec.z * vec.z, 0.5f);

        return dis;
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

    // TaskNode 모음
    private ReturnCode Attack()
    {
        if (UseLookAt)
        {
            transform.LookAt(target.transform);
            UseLookAt = false;
            monsterAnimation.SetTrigger("IsAttack");
            ChildAttack();
        }
  
        return ReturnCode.SUCCESS;
    }

    // 자식에게서 추가 행동이 있을 경우
    protected virtual void ChildAttack() {}

    private ReturnCode MoveToPosition()
    {
        monsterAnimation.SetBool("IsMove", true);
        transform.LookAt(target.transform);
        float LerpT = blackBoard.m_MoveSpeed.Key * Time.deltaTime / Vector3.Distance(gameObject.transform.position, target.transform.position);
        //transform.position = Vector3.Lerp(gameObject.transform.position, target.transform.position, LerpT);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, blackBoard.m_MoveSpeed.Key * Time.deltaTime);
        Vector3 fixYPos = new Vector3(transform.position.x, initialY, transform.position.z);
        transform.position = fixYPos;

        return ReturnCode.SUCCESS;
    }

    private ReturnCode IsHPZero()
    {
        if (blackBoard.m_HP.Key <= 0)
        {
            monsterAnimation.SetBool("IsDeath", true);
            StartCoroutine(DecreaseAlpha());
            IsDeath = true;
            BaseMonster.monsterDeathEvent.Invoke();
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAIL;
        }
    }

    // 유니티 이벤트 모음
    // CollisionCheck의 이벤트 용
    public void OnHitEvent(Collider other)
    {
        if (other.gameObject == null) return;

        if (!IsHit && attackAnimationSpan && !IsDeath)
        {
            BaseObject otherObject = other.gameObject.GetComponent<BaseObject>();
            otherObject.blackBoard.m_HP.Key -= blackBoard.m_DefaultAttackDamage.Key;
            IsHit = true;
            OnChildHitEvent();
        }
    }

    // 자식에게서 추가 행동이 있을 경우
    protected virtual void OnChildHitEvent() {}

    // SearchCollision의 이벤트 용
    public void OnSearchCollisionEvent(Collider other)
    {
        if (target != null)
        {
            BaseObject obj = target.gameObject.GetComponent<BaseObject>();
            if (!obj.IsDeath && obj.SelfType == ObjectType.Monster) return;
        }
       
        target = other.gameObject;
        blackBoard.m_TargetObject.Key = target;
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
