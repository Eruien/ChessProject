using UnityEngine;

public class ColliderExclude : MonoBehaviour
{
    private int teamLayerNumber = 0;
    Collider attackCollider;
   
    private void Awake()
    {
        attackCollider = GetComponent<BoxCollider>();
        if (attackCollider == null)
        {
            attackCollider = GetComponent<SphereCollider>();
        }
    }

    private void Start()
    {
        teamLayerNumber = transform.root.gameObject.layer;
        attackCollider.excludeLayers = (1 << teamLayerNumber) | attackCollider.excludeLayers;
    }
}
