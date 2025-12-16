using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> moveTo;
    public int startIndex;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            print("it works!!!");
    }
}