using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Scriptable Objects/Create new item")]
public class ItemInfo : ScriptableObject {
    [Header("name/tag")]
    public string itemTag;
    public GameObject item;
    [Header("setings")]
    public List<ItemInfo> ingrediants;
}
