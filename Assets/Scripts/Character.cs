using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    [SerializeField]
    public float viewHP = 0.0f;

    public UnityEvent deathEvent;
    public BlackBoard beeBlackBoard = new BlackBoard();
   
    private Material material;
    private Animator beeAnimation;
    
    private Selector selector;

    private bool IsDeath = false;
    private bool IsHit = false;
    private float beeAlpha = 1.0f;
   
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
        MonsterManager.Instance.UnRegister(gameObject);
    }

    private ReturnCode FlyAttack()
    {
        beeAnimation.SetTrigger("IsAttack");
        return ReturnCode.SUCCESS;
    }

    private ReturnCode MoveToPosition()
    {
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, 0.01f);
        
        return ReturnCode.SUCCESS;
    }

    private ReturnCode IsHPZero()
    {
        if (beeBlackBoard.m_HP.Key <= 0)
        {
            Debug.Log("HP Zero EXECUTE");
            beeAnimation.SetBool("IsDeath", true);
            StartCoroutine(DecreaseAlpha());
            IsDeath = true;
            return ReturnCode.SUCCESS;
        }
        else
        {
            return ReturnCode.FAIL;
        }
    }

    private void SetBlackBoardKey()
    {
        beeBlackBoard.m_HP.Key = 100.0f;
        beeBlackBoard.m_SearchRange.Key = 20.0f;
        beeBlackBoard.m_AttackDistance.Key = ComputeAttackDistance();
        beeBlackBoard.m_AttackRange.Key = 1.5f;
        beeBlackBoard.m_TargetObject.Key = target;
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

    private float ComputeAttackDistance()
    {
        if (target == null) return 100.0f;
        Vector3 vec = target.transform.position - transform.position;
        float dis = Mathf.Pow(vec.x * vec.x + vec.z * vec.z, 0.5f);

        return dis;
    }

    private void OnEnable()
    {
        deathEvent.AddListener(OnDeathEvent);
    }

    private void Awake()
    {
        MonsterManager.Instance.Register(gameObject);
        material = FindMaterial();
        beeAnimation = GetComponentInChildren<Animator>();
        SetBlackBoardKey();
        
        selector = new Selector();
    }

    private void Start()
    {
        // HP관리
        Selector HPMgr = new Selector();
        selector.AddChild(HPMgr);

        Sequence<float, b_float> checkHPZero = new Sequence<float, b_float>(KeyQuery.IsLessThanOrEqualTo, 0.001f, beeBlackBoard.m_HP);
        HPMgr.AddChild(checkHPZero);

        Action action = new Action(IsHPZero);
        checkHPZero.AddChild(action);

        // 공격 관리
        Selector attackMgr = new Selector();
        selector.AddChild(attackMgr);

        Sequence<float, b_float> checkAttackRange = new Sequence<float, b_float>(KeyQuery.IsLessThanOrEqualTo, beeBlackBoard.m_AttackRange.Key, beeBlackBoard.m_AttackDistance);
        attackMgr.AddChild(checkAttackRange);

        Action attackAction = new Action(FlyAttack);
        checkAttackRange.AddChild(attackAction);

        // 이동 관리
        Selector moveMgr = new Selector();
        selector.AddChild(moveMgr);

        SetSequence<GameObject, b_GameObject> move = new SetSequence<GameObject, b_GameObject>(KeyQuery.IsSet, beeBlackBoard.m_TargetObject);
        moveMgr.AddChild(move);
        Action moveAction = new Action(MoveToPosition);

        move.AddChild(moveAction);
        //StartCoroutine(HPReduce(beeBlackBoard.m_HP));
    }

    private void Update()
    {
        viewHP = beeBlackBoard.m_HP.Key;
        
        if (!IsDeath)
        {
            beeBlackBoard.m_AttackDistance.Key = ComputeAttackDistance();
            selector.Tick();
        }
        else
        {
            StartCoroutine(DeathAndDestroy(1.0f));
        }
    }

    private void OnDisable()
    {
        MonsterManager.UnityDeathEvent.RemoveListener(OnDeathEvent);
    }

    // CollisionCheck의 이벤트 용
    public void OnHitEvent(Collider other)
    {
        if (!IsHit)
        {
            Character otherCharacter = other.gameObject.GetComponent<Character>();
            otherCharacter.beeBlackBoard.m_HP.Key -= 5;
            IsHit = true;
        }
    }

    // SearchCollision의 이벤트 용
    public void OnSearchCollisionEvent(Collider other)
    {
        target = other.gameObject;
        beeBlackBoard.m_TargetObject.Key = target;
    }

    // 타격을 한 번만 입히게 하기 위해
    // AttackAnimationCheck의 이벤트 용
    public void OnAttackAnimationStart()
    {
        IsHit = false;
    }

    // MonsterManager의 DeathEvent 용
    public void OnDeathEvent()
    {
        if (target == null)
        {
            Debug.Log("타겟이 null이야");
        }
    }
}
