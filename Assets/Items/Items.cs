using System;
using NaughtyAttributes;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Expandable]
    public ItemInfo itemInfo;
    public Rigidbody rb;
    void Update() {
        float sped = 100;
        if (transform.parent != null) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * sped);
            rb.isKinematic = true;
        }
        else rb.isKinematic = false;

        transform.LookAt(Camera.main.transform);
    }

    void Start()
    {
        bool isInList = false;

        for(int i = 0; i < GameController.gameController.items.Count; i++)
            if (GameController.gameController.items[i] == gameObject)
                isInList = true;

        if(!isInList)
            GameController.gameController.items.Add(gameObject);
    }
}