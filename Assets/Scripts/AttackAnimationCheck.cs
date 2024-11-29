using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackAnimationCheck : MonoBehaviour
{
    [SerializeField]
    public UnityEvent attackAnimationStartEvent;

    private void OnAttackStart()
    {
        attackAnimationStartEvent.Invoke();
    }
   
}
