using System;
using NaughtyAttributes;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Expandable]
    public ItemInfo itemInfo;
    void Update() {
        float sped = 100;
        if (transform.parent != null) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * sped);
        }
    }

    void Start()
    {

    }
}