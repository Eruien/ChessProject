using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    private ResourceManager _resource = new ResourceManager();
    private MonsterManager _monster = new MonsterManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static MonsterManager Monster { get { return Instance._monster; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();
            
            // 매니저들 초기화 작업
        }
    }

    public static void Clear()
    {
        // 매니저들 Clear 작업
    }
}
