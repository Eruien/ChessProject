using Assets.Scripts;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class Character : BaseObject
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
    private Animator beeAnimation;
    private CapsuleCollider selfCollider;
    private Selector selector;

    private bool IsHit = false;
    private bool UseLookAt = true;
    private float beeAlpha = 1.0f;

    // 유니티 라이프 사이클 함수 
    protected void Awake()
    {
        Managers.Monster.Register(gameObject);
        material = FindMaterial();
        beeAnimation = GetComponentInChildren<Animator>();
        selfCollider = GetComponent<CapsuleCollider>();
        targetLabo = GameObject.Find("Labo");
        SetBlackBoardKey();

        selector = new Selector();
    }

    protected void OnEnable()
    {
        Character.monsterDeathEvent.AddListener(OnDeathEvent);
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

        Action attackAction = new Action(FlyAttack);
        checkAttackRange.AddChild(attackAction);

        // 이동 관리
        Selector moveMgr = new Selector();
        selector.AddChild(moveMgr);

        SetSequence<GameObject, b_GameObject> move = new SetSequence<GameObject, b_GameObject>(KeyQuery.IsSet, blackBoard.m_TargetObject);
        moveMgr.AddChild(move);
        Action moveAction = new Action(MoveToPosition);

        move.AddChild(moveAction);
        //StartCoroutine(HPReduce(beeBlackBoard.m_HP));
    }

    protected void Update()
    {
        viewHP = blackBoard.m_HP.Key;
        if (IsNotDeath)
        {
            blackBoard.m_HP.Key = 100;
        }


        if (!IsDeath)
        {
            if (target == null)
            {
                if (targetLabo is not null)
                {
                    target = targetLabo;
                    blackBoard.m_TargetObject.Key = target;
                }
            }
            blackBoard.m_AttackDistance.Key = ComputeAttackDistance();
            selector.Tick();
        }
        else
        {
            StartCoroutine(DeathAndDestroy(1.0f));
        }
    }

    protected void OnDisable()
    {
        Character.monsterDeathEvent.RemoveListener(OnDeathEvent);
    }

    protected override void SetBlackBoardKey()
    {
        DataManager.Instance.FetchData();
        blackBoard.m_HP.Key = 100.0f;
        blackBoard.m_SearchRange.Key = 20.0f;
        blackBoard.m_AttackDistance.Key = ComputeAttackDistance();
        blackBoard.m_AttackRange.Key = 1.5f;
        target = targetLabo;
        blackBoard.m_TargetObject.Key = target;
    }

    protected float ComputeAttackDistance()
    {
        if (target == null) return 100.0f;
        Vector3 vec = target.transform.position - transform.position;
        float dis = Mathf.Pow(vec.x * vec.x + vec.z * vec.z, 0.5f);

        return dis;
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
        while (beeAlpha >= 0)
        {
            beeAlpha -= 0.1f;
            material.SetFloat("_Alpha", beeAlpha);  // 셰이더에 알파 값을 전달
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DeathAndDestroy(float deathTime)
    {
        yield return new WaitForSeconds(deathTime);
        Managers.Monster.UnRegister(gameObject);
    }

    // TaskNode 모음
    
    private ReturnCode FlyAttack()
    {
        if (UseLookAt)
        {
            transform.LookAt(target.transform);
            UseLookAt = false;
            beeAnimation.SetTrigger("IsAttack");
            ChildAttack();
        }
  
        return ReturnCode.SUCCESS;
    }

    protected virtual void ChildAttack() { }

    private ReturnCode MoveToPosition()
    {
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, 0.01f);
        
        return ReturnCode.SUCCESS;
    }

    private ReturnCode IsHPZero()
    {
        if (blackBoard.m_HP.Key <= 0)
        {
            Debug.Log("HP Zero EXECUTE");
            beeAnimation.SetBool("IsDeath", true);
            StartCoroutine(DecreaseAlpha());
            IsDeath = true;
            Character.monsterDeathEvent.Invoke();
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
        if (!IsHit)
        {
            BaseObject otherObject = other.gameObject.GetComponent<BaseObject>();
            otherObject.blackBoard.m_HP.Key -= 50;
            IsHit = true;
        }
    }

    // SearchCollision의 이벤트 용
    public void OnSearchCollisionEvent(Collider other)
    {
        target = other.gameObject;
        blackBoard.m_TargetObject.Key = target;
    }

    // 타격을 한 번만 입히게 하기 위해
    // AttackAnimationCheck의 이벤트 용
    public void OnAttackAnimationStart()
    {
        IsHit = false;
    }

    // 애니메이션 끝나는 타이밍 체크
    public void OnAttackAnimationEnd()
    {
        UseLookAt = true;
    }

    // MonsterManager의 DeathEvent 용
    public void OnDeathEvent()
    {
        Debug.Log("죽는 이벤트가 모두에게 전달되는지 테스트");

        if (!IsDeath)
        {
            selfCollider.enabled = false;
            selfCollider.enabled = true;
        }
    }
}