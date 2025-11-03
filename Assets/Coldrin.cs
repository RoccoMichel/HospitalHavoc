using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coldrin : MonoBehaviour {
    public List<ItemInfo> allPosibolItems; // fore ez´er item adishen

    public static List<ItemInfo> AllPosibolItems; // list of all posibol items
    public static List<ItemInfo> Ingerdens = new List<ItemInfo>(); // all the items curenty in the coldrin

    void Start() {
        AllPosibolItems = allPosibolItems;
    }
    // adds an igedent to the coldrin
    public static void AddIngedent(ItemInfo info) {
        Ingerdens.Add(info);
    }

    // mixes all ingedenst that ar in the coldrin
    public static GameObject MixIngedents() {
        for (int i = 0; i < AllPosibolItems.Count; i++) {
            if (AllPosibolItems[i].ingrediants == Ingerdens)
                return Instantiate(AllPosibolItems[i].item, Vector3.zero, Quaternion.identity);
        }

        return AllPosibolItems[0].item; // If resepy dusent exist
    }
}
