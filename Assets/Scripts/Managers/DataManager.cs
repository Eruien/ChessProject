using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDict<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

[Serializable]
public class MonsterStat
{
    public string name;
    public float hp;
    public float attackRange;
    public float attackRangeCorrectionValue;
    public float attackDistance;
    public float speed;
}

[Serializable]
public class MonsterData : IDict<string, MonsterStat>
{
    public List<MonsterStat> monsterStat = new List<MonsterStat>();

    public Dictionary<string, MonsterStat> MakeDict()
    {
        Dictionary<string, MonsterStat> dict = new Dictionary<string, MonsterStat>();
        foreach (MonsterStat stat in monsterStat)
        {
            dict.Add(stat.name, stat);
        }
        return dict;
    }
}

public class DataManager
{
    public Dictionary<string, MonsterStat> monsterDict { get; private set; } = new Dictionary<string, MonsterStat>();
    
    public void Init()
    {
        monsterDict = LoadJson<MonsterData, string, MonsterStat>("MonsterData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : IDict<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"GameData/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
