using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackAnimationCheck : MonoBehaviour
{
    [SerializeField]
    public UnityEvent attackAnimationStartEvent;

    [SerializeField]
    public UnityEvent attackAnimationEndEvent;

    private void OnAttackStart()
    {
        attackAnimationStartEvent.Invoke();
    }

    private void OnAttackEnd()
    {
        attackAnimationEndEvent.Invoke();
    }

}
