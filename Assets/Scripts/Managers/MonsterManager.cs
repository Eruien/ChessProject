using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private List<GameObject> MonsterList = new List<GameObject>(); 

    public void Register(GameObject obj)
    {
        MonsterList.Add(obj);
    }

    public void UnRegister(GameObject obj)
    {
        MonsterList.Remove(obj);
        Object.Destroy(obj);
        obj = null;
    }
}
