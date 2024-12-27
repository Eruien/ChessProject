using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private Dictionary<int, GameObject> MonsterList = new Dictionary<int, GameObject>(); 

    public void Register(int monsterCount, GameObject obj)
    {
        MonsterList.Add(monsterCount, obj);
    }

    public void UnRegister(GameObject obj)
    {
        //MonsterList.Remove(obj);
        Object.Destroy(obj);
        obj = null;
    }

    public GameObject GetMonster(int monsterId)
    {
        GameObject obj = null;
        if (MonsterList.TryGetValue(monsterId, out obj))
        {
            return obj;
        }
        
       return null;
    }
}
