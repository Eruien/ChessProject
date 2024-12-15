using Assets.Scripts;
using UnityEngine;
using System;

public class ProjectTile : MonoBehaviour
{
    public b_GameObject TargetObject { get; set; }
    public float AttackRange { get; set; }
    public float AttackRangeCorrectionValue { get; set; }
    public float ProjectTileSpeed { get; set; }
    public bool IsUse { get; set; } = true;
    private Vector3 initialPos = Vector3.zero;
    
    private void Awake()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (TargetObject.Key == null)
        {
            IsUse = false;
        }
        else
        {
            Vector3 direction = (TargetObject.Key.transform.position - transform.position).normalized;
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
