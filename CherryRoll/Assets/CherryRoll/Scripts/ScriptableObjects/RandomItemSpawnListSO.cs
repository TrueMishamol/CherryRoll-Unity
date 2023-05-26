using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RandomItemSpawnListSO : ScriptableObject {


    [SerializeField] public List<RandomItem> RandomItemSpawnList;


    [System.Serializable]
    public class RandomItem {
        [SerializeField] public ItemSO itemSO;
        [SerializeField] public int rarity;
    }
}
