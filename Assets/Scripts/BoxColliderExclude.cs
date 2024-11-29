using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderExclude : MonoBehaviour
{
    private int teamLayerNumber = 0;
    BoxCollider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        teamLayerNumber = transform.root.gameObject.layer;
        attackCollider.excludeLayers = (1 << teamLayerNumber) | attackCollider.excludeLayers;
    }
}
