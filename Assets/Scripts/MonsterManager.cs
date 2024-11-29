using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MonsterManager : MonoBehaviour
{
    static public UnityEvent UnityDeathEvent = new UnityEvent();

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

    private List<Object> MonsterList = new List<Object>(); 

    public void Register(Object obj)
    {
        MonsterList.Add(obj);
    }

    public void UnRegister(Object obj)
    {
        MonsterList.Remove(obj);
        Destroy(obj);
        obj = null;
        UnityDeathEvent.Invoke();
    }
}
