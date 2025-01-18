using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 vec = GameObject.Find("RedTeamSpawnSpot").GetComponent<MeshCollider>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
