using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField]
    public UnityEvent<Collider> UnityHitEvent;

    public UnityAction<Collider> hitAction;

    private void OnTriggerEnter(Collider other)
    {
        UnityHitEvent.Invoke(other);
    }

    private void OnDisable()
    {
        if (hitAction == null) return;
        UnityHitEvent.RemoveListener(hitAction);
    }

    public void CollisionAddListener(UnityAction<Collider> action)
    {
        hitAction += action;
        UnityHitEvent.AddListener(hitAction);
    }
}
