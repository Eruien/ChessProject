using Assets.Scripts;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBall : MonoBehaviour
{
    public b_GameObject TargetObject { get; set; }
    public float AttackRange { get; set; }
    public float AttackRangeCorrectionValue { get; set; }
    private Vector3 initialPos = Vector3.zero;
   
    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (TargetObject.Key == null)
        {
            Destroy(gameObject);
        }
      
        Vector3 direction = (TargetObject.Key.transform.position - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(direction * 10.0f);

        if (ComputeDistance() >= AttackRange + AttackRangeCorrectionValue)
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
