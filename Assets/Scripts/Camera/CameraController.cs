using Assets.Scripts;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject RedPlane = null;
    private GameObject BluePlane = null;
    private Vector3 planeMax = Vector3.zero;
    private Vector3 planeMin = Vector3.zero;
    private Vector3 redTeamStartCameraPos = new Vector3(0.0f, 25.0f, -12.0f);
    private Vector3 redTeamStartCameraRot = new Vector3(90.0f, 0.0f, 0.0f);
    private Vector3 blueTeamStartCameraPos = new Vector3(0.0f, 25.0f, 12.0f);
    private Vector3 blueTeamStartCameraRot = new Vector3(90.0f, -180.0f, 0.0f);
    private float cameraSpeed = 20.0f;
    private float scrollMultiplier = 100.0f;
    private float cameraRotationSpeed = 100.0f;

    private void Awake()
    {
        RedPlane = GameObject.Find("RedPlane");
        BluePlane = GameObject.Find("BluePlane");
        planeMax = BluePlane.GetComponent<MeshCollider>().bounds.max;
        planeMin = RedPlane.GetComponent<MeshCollider>().bounds.min;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D
        float vertical = Input.GetAxis("Vertical"); // W, S
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // ¸¶¿ì½º ÈÙ
        float rotationDir = cameraRotationSpeed * Time.deltaTime;

        Vector3 direction = new Vector3(horizontal, vertical, scroll * scrollMultiplier);
        transform.Translate(direction * cameraSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationDir, Space.World); 
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationDir, Space.World); 
        }

        LimitCameraRange();
    }

    public void SetTeamCameraTransform(Team team)
    {
        if (team == Team.RedTeam)
        {
            transform.position = redTeamStartCameraPos;

            transform.eulerAngles = redTeamStartCameraRot;
            return;
        }

        transform.position = blueTeamStartCameraPos;
        transform.eulerAngles = blueTeamStartCameraRot;
    }

    private void LimitCameraRange()
    {
        if (transform.position.x >= planeMax.x || transform.position.z >= planeMax.z)
        {
            float maxX = Mathf.Min(transform.position.x, planeMax.x);
            float maxZ = Mathf.Min(transform.position.z, planeMax.z);
            transform.position = new Vector3(maxX, transform.position.y, maxZ);
        }

        if (transform.position.x <= planeMin.x || transform.position.z <= planeMin.z)
        {
            float minX = Mathf.Max(transform.position.x, planeMin.x);
            float minZ = Mathf.Max(transform.position.z, planeMin.z);
            transform.position = new Vector3(minX, transform.position.y, minZ);
        }
    }    
}
