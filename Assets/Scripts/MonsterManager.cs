using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    
    public static MonsterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Hierarchy에 Singleton 오브젝트가 없으면 생성
                GameObject singletonObject = new GameObject(typeof(MonsterManager).Name);
                _instance = singletonObject.AddComponent<MonsterManager>();
                DontDestroyOnLoad(singletonObject); // 씬 전환 시 파괴 방지
            }
            return _instance;
        }
    }

    private List<GameObject> MonsterList = new List<GameObject>(); 

    public void Register(GameObject obj)
    {
        MonsterList.Add(obj);
    }

    public void UnRegister(GameObject obj)
    {
        MonsterList.Remove(obj);
        Destroy(obj);
        obj = null;
    }
}
