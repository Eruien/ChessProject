using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderExclude : MonoBehaviour
{
    private int teamLayerNumber = 0;
    SphereCollider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        teamLayerNumber = transform.root.gameObject.layer;
        attackCollider.excludeLayers = (1 << teamLayerNumber) | attackCollider.excludeLayers;
    }
}
