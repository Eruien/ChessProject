using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    public List<GameObject> SpawnPanelList = new List<GameObject>();
    private Vector3 RedTeamSpawnStartPos { get; set; } = new Vector3();
    private Vector3 BlueTeamSpawnStartPos { get; set; } = new Vector3();

    private Vector3 RedTeamCurrentSpawnPoint = new Vector3();
    private Vector3 BlueTeamCurrentSpawnPoint = new Vector3();
    private Vector3 RedTeamDir = new Vector3(1, 1, -1);
    private Vector3 BlueTeamDir = new Vector3(-1, 1, 1);

    private float RowSize { get; set; } = 12.5f;
    

    public void Init()
    {
        RedTeamSpawnStartPos = GameObject.Find("RedTeamSpawnStart").transform.position;
        BlueTeamSpawnStartPos = GameObject.Find("BlueTeamSpawnStart").transform.position;
        RedTeamCurrentSpawnPoint = RedTeamSpawnStartPos;
        BlueTeamCurrentSpawnPoint = BlueTeamSpawnStartPos;
    }

    public Vector3 ComputeSpawnPoint(Team team, string monsterType)
    {
        BoxCollider box = SearchPanelGameObject(Managers.Resource.Load<GameObject>($"Prefabs/{monsterType}"), "SpawnPlane").GetComponent<BoxCollider>();
        Vector3 panelSize = new Vector3
            (box.size.x * box.gameObject.transform.localScale.x,
            box.size.y * box.gameObject.transform.localScale.y,
            box.size.z * box.gameObject.transform.localScale.z);
        Vector3 spawnPoint = new Vector3();
        
        if (team == Team.RedTeam)
        {
            if (Mathf.Abs(RedTeamCurrentSpawnPoint.x - RedTeamSpawnStartPos.x) >= RowSize)
            {
                RedTeamCurrentSpawnPoint.x = RedTeamSpawnStartPos.x;
                RedTeamCurrentSpawnPoint.z += panelSize.z * RedTeamDir.z;
            }
            spawnPoint = RedTeamCurrentSpawnPoint;
            RedTeamCurrentSpawnPoint.x += panelSize.x * RedTeamDir.x;
            return spawnPoint;
        }

        if (Mathf.Abs(BlueTeamCurrentSpawnPoint.x - BlueTeamSpawnStartPos.x) >= RowSize)
        {
            BlueTeamCurrentSpawnPoint.x = BlueTeamSpawnStartPos.x;
            BlueTeamCurrentSpawnPoint.z += panelSize.z * BlueTeamDir.z;
        }
        spawnPoint = BlueTeamCurrentSpawnPoint;
        BlueTeamCurrentSpawnPoint.x += panelSize.x * BlueTeamDir.x;
        return spawnPoint;
    }

    public GameObject SearchPanelGameObject(GameObject obj, string objName)
    {
        Transform[] allObjects = obj.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in allObjects)
        {
            if (child.gameObject.name == objName)
            {
                return child.gameObject;
            }
        }

        return null;
    }

    public void RegisterPanel(GameObject obj)
    {
        SpawnPanelList.Add(obj);
    }

    public void RegisterPanelAllClear()
    {
        SpawnPanelList.Clear();
    }
}
