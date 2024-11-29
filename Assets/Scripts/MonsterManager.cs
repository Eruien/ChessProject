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
                // Hierarchy�� Singleton ������Ʈ�� ������ ����
                GameObject singletonObject = new GameObject(typeof(MonsterManager).Name);
                _instance = singletonObject.AddComponent<MonsterManager>();
                DontDestroyOnLoad(singletonObject); // �� ��ȯ �� �ı� ����
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
