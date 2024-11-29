using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionCheck : MonoBehaviour
{
    [SerializeField]
    public UnityEvent<Collider> UnityHitEvent;

    private void OnTriggerEnter(Collider other)
    {
        UnityHitEvent.Invoke(other);
    }
}
