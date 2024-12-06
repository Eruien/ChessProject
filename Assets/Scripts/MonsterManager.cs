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
                // Hierarchy�� Singleton ������Ʈ�� ������ ����
                GameObject singletonObject = new GameObject(typeof(MonsterManager).Name);
                _instance = singletonObject.AddComponent<MonsterManager>();
                DontDestroyOnLoad(singletonObject); // �� ��ȯ �� �ı� ����
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
