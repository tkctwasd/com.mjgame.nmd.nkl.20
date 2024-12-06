using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Data/PoolConfigSO", order = 0)]
public class PoolConfigSO : ScriptableObject
{
    public List<PoolItem> poolItems;
}

[System.Serializable]
public class PoolItem
{
    public string name;
    public GameObject prefab;
    public int size=3;
}
// Compare this snippet from Assets/Module/Pool/_ROOT/Scripts/PoolConfigSO.cs: