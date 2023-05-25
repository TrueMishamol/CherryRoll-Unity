using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MagicTableclothItemsListSO : ScriptableObject {


    [SerializeField] public List<MagicTableclothItem> magicTableclothItemsList;


    [System.Serializable]
    public class MagicTableclothItem {
        [SerializeField] public ItemSO itemSO;
        [SerializeField] public int value;
        [SerializeField] public int rarity;
    }
}
