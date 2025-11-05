using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Expandable]
    public ItemInfo itemInfo;
    public Rigidbody rb;

    public PlayerController owner;

    void Update() {
        float sped = 100;
        if (transform.parent != null) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * sped);
            rb.isKinematic = true;
        }
        else rb.isKinematic = false;

        transform.LookAt(Camera.main.transform);
    }

    void OnCollisionEnter(Collision other)
    {
        if (transform.parent == null)
        {
            if(other.gameObject.CompareTag("Player"))
                other.gameObject.GetComponent<PlayerController>().PickUpChosenItem(gameObject);
            else if (CheckIfInList(GameController.gameController.interactables, other.gameObject))
            {
                if(other.gameObject.GetComponent<Interact>().itemCanInteract && owner)
                    other.gameObject.GetComponent<Interact>().onInteract.Invoke(owner);
            }
        }
    }

    void Awake()
    {
        bool isInList = false;

        for(int i = 0; i < GameController.gameController.items.Count; i++)
            if (GameController.gameController.items[i] == gameObject)
                isInList = true;

        if(!isInList)
            GameController.gameController.items.Add(gameObject);
    }

    public bool CheckIfInList(List<GameObject> list, GameObject objectInList)
    {
        bool isInList = false;

        foreach (GameObject obj in list)
            if (obj == objectInList)
                isInList = true;

        return isInList;
    }
}