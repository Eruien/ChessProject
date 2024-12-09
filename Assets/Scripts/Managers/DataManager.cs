using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    public float hp;
    public float attackRange;
}


public class DataManager
{
    private static DataManager _instance = new DataManager();

    public static DataManager Instance { get { return _instance; } }

    public void FetchData()
    {
        TextAsset asset = Resources.Load<TextAsset>($"GameData/BattleBee");
        Stats data = JsonUtility.FromJson<Stats>(asset.text);
        Debug.Log(asset.text);
    }
}
