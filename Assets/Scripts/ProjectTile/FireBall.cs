using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FireBall : MonoBehaviour
{
    public b_GameObject TargetObject { get; set; }
   
    private void Start()
    {
    }

    private void Update()
    {
        Vector3 direction = (TargetObject.Key.transform.position - transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(direction * 10.0f);
    }
}
