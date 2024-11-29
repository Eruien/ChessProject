using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchCollision : MonoBehaviour
{
    [SerializeField]
    public UnityEvent<Collider> UnitySearchEvent;

    private void OnTriggerEnter(Collider other)
    {
        UnitySearchEvent.Invoke(other);
    }
}
