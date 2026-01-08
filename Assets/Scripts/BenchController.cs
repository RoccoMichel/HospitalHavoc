using System;
using System.Collections.Generic;
using UnityEngine;

public class BenchController : MonoBehaviour
{
    public List<Transform> itemPlaces;
    public List<GameObject> itemsOnBench;

    void Update()
    {
        if (itemsOnBench.Count == itemPlaces.Count)
        {
            GetComponent<Interact>().needsItem = false;
            GetComponent<Interact>().needsEmptyHand = true;
            return;
        }
        else
        {
            GetComponent<Interact>().needsItem = true;
            GetComponent<Interact>().needsEmptyHand = false;
        }

        if (itemsOnBench.Count > 0)
            GetComponent<Interact>().needsItem = false;
        else
            GetComponent<Interact>().needsItem = true;
    }
}