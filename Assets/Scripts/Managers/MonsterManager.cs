using System.Collections.Generic;
using UnityEngine;

public class MonsterManager
{
    private int monsterCount = 0;
    private Dictionary<int, GameObject> MonsterList = new Dictionary<int, GameObject>(); 

    public int Register(GameObject obj)
    {
        monsterCount++;
        MonsterList.Add(monsterCount, obj);
        return monsterCount;
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
