using Assets.Scripts;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    public b_GameObject TargetObject { get; set; } = new b_GameObject();
    public float AttackRange { get; set; }
    public float AttackRangeCorrectionValue { get; set; }
    public float ProjectTileSpeed { get; set; }
    public bool IsUse { get; set; } = true;

    private CapsuleCollider targetCollider;
    private Vector3 initialPos = Vector3.zero;

    private float startTime = 0.0f;
    private float endTime = 5.0f;
    
    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Start()
    {
      
    }

    private void Update()
    {
        startTime += Time.deltaTime;

        if (startTime >= endTime)
        {
            IsUse = false;
        }
        
        if (TargetObject.Key == null)
        {
            IsUse = false;
        }
        else
        {
            Vector3 direction = (TargetObject.Key.GetComponent<CapsuleCollider>().bounds.center - transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(direction * ProjectTileSpeed);
        }
      
        if (ComputeDistance() >= AttackRange + AttackRangeCorrectionValue)
        {
            IsUse = false;
        }

        if (!IsUse)
        {
            Destroy(gameObject);
        }
    }

    private float ComputeDistance()
    {
        Vector3 vec = transform.position - initialPos;
        float dis = Mathf.Pow(vec.x * vec.x + vec.z * vec.z, 0.5f);
        return dis; 
    }
}
