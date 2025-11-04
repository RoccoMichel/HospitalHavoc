using System;
using System.Collections.Generic;
using UnityEngine;

public class Coldrin : MonoBehaviour
{
    public static Coldrin inctanse;

    public List<ItemInfo> AllPosibolItems; // list of all posibol items
    public List<ItemInfo> Ingerdens = new List<ItemInfo>(); // all the items curenty in the coldrin

    // adds an igedent to the coldrin
    public void AddIngedent(ItemInfo info) {
        Ingerdens.Add(info);
    }

    // mixes all ingedenst that ar in the coldrin
    public GameObject MixIngedents() {
        print("wtf");
        for (int i = 0; i < AllPosibolItems.Count; i++) {
            if (AllPosibolItems[i].ingrediants == Ingerdens)
                return Instantiate(AllPosibolItems[i].item, Vector3.zero, Quaternion.identity);
        }

        return Instantiate(AllPosibolItems[0].item); // If resepy dusent exist
    }

    void Awake()
    {
        inctanse = this;
    }
}