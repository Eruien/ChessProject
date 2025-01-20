using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class DragObject : MonoBehaviour
{
    static public UnityEvent objectSelectEvent = new UnityEvent();

    private GameObject SpawnPanel = null;
    private MeshRenderer SpawnPanelMeshRender = null;
    private Camera mainCamera = null;
    private Vector3 offset = Vector3.zero; // 마우스와 오브젝트 사이의 거리
    private Vector3 upPos = new Vector3(0.0f, 3.0f, 0.0f);
    private Plane dragPlane; // 드래그 평면
    private PanelState currentState = PanelState.None;
    private float InitialY = 0.0f;
    private float fixValue = -0.1f;
    private bool IsGreen = false;
  
    private void OnEnable()
    {
        DragObject.objectSelectEvent.AddListener(OnSelectInitialize);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        SpawnPanel = Managers.Spawn.SearchPanelGameObject(gameObject, "SpawnPlane");
        SpawnPanelMeshRender = SpawnPanel.GetComponent<MeshRenderer>();
        InitialY = transform.position.y;
    }

    private void Update()
    {
        if (currentState == PanelState.SelectMove)
        {
            if (ComputePlaneCollision())
            {
                SpawnPanelMeshRender.material = Managers.Resource.Load<Material>($"Material/SpawnOrange");
                IsGreen = false;
            }
            else
            {
                SpawnPanelMeshRender.material = Managers.Resource.Load<Material>($"Material/SpawnGreen");
                IsGreen = true;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentState)
            {
                case PanelState.None:
                    SelectObject();
                    break;
                case PanelState.Select:
                    ConfirmSelectObjectClick();
                    break;
                case PanelState.SelectMove:
                    UnSelectObject();
                    break;
            }
        }

        // 선택 상태일 때 마우스 움직임에 따라 오브젝트 이동
        if (currentState == PanelState.SelectMove)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            // 마우스가 드래그 평면과 교차하는 지점 계산
            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 newPosition = ray.GetPoint(distance) + offset;
                transform.position = newPosition + upPos;
            }
        }  
    }

    private void OnDisable()
    {
        DragObject.objectSelectEvent.RemoveListener(OnSelectInitialize);
    }

    private void SelectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            objectSelectEvent.Invoke();
            currentState = PanelState.Select;
            SpawnPanelMeshRender.material = Managers.Resource.Load<Material>($"Material/SpawnGreen");
        }
    }

    private void UnSelectObject()
    {
        if (IsGreen)
        {
            currentState = PanelState.None;
            SpawnPanelMeshRender.material = Managers.Resource.Load<Material>($"Material/SpawnDefault");
            transform.position = new Vector3(transform.position.x, InitialY, transform.position.z);
        }
    }

    private void ConfirmSelectObjectClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            currentState = PanelState.SelectMove;
            // 드래그 평면 설정
            dragPlane = new Plane(Vector3.up, transform.position);

            // 마우스와 오브젝트의 거리 계산
            dragPlane.Raycast(ray, out float distance);
            offset = transform.position - ray.GetPoint(distance);
        }
    }

    private bool ComputePlaneCollision()
    {
        foreach (var arg in Managers.Spawn.SpawnPanelList)
        {
            if (SpawnPanel == arg) continue;
            BoxCollider box = SpawnPanel.GetComponent<BoxCollider>();
            BoxCollider otherBox = arg.GetComponent<BoxCollider>();
            if (Mathf.Abs(otherBox.bounds.center.x - box.bounds.center.x) <= 
                ((box.size.x * SpawnPanel.gameObject.transform.localScale.x * 0.5f + otherBox.size.x * arg.gameObject.transform.localScale.x * 0.5f) + fixValue) &&
                Mathf.Abs(otherBox.bounds.center.z - box.bounds.center.z) <= 
                ((box.size.z *SpawnPanel.gameObject.transform.localScale.z * 0.5f + otherBox.size.z * arg.gameObject.transform.localScale.z * 0.5f)) + fixValue)
            {
                return true;
            }
        }

        return false;
    }

    private void OnSelectInitialize()
    {
        currentState = PanelState.None;
        SpawnPanelMeshRender.material = Managers.Resource.Load<Material>($"Material/SpawnDefault");
    }
}
