using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public Dictionary<int, GameObject> objectList = new Dictionary<int, GameObject>(); 

    public void Register(int monsterCount, GameObject obj)
    {
        objectList.Add(monsterCount, obj);
    }

    public void UnRegister(GameObject obj)
    {
        objectList.Remove(obj.GetComponent<BaseObject>().ObjectId);
        Object.Destroy(obj);
        obj = null;
    }

    public GameObject GetMonster(int objectId)
    {
        GameObject obj = null;
        if (objectList.TryGetValue(objectId, out obj))
        {
            return obj;
        }
        
       return null;
    }
}
