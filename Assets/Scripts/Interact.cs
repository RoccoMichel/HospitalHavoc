using System;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent<PlayerController> onInteract;

    public bool needsEmptyHand = true, itemCanInteract = false;

    public void ColdrinInteract(PlayerController player)
    {
        if (player.currentHeldItem != null)
        {
            Coldrin.inctanse.AddIngedent(player.currentHeldItem.itemInfo);
            GameController.gameController.items.Remove(player.currentHeldItem.gameObject);
            Destroy(player.currentHeldItem.gameObject);
            player.currentHeldItem = null;
        }
        else
            Coldrin.inctanse.MixIngedents();
    }

    public void SpawnItem(PlayerController player)
    {
        Debug.Log("Spone item");
        GameObject spawnedItem = GetComponent<SponeItem>().sponeItem();

        player.PickUpChosenItem(spawnedItem);
    }

    public void TryCure(PlayerController player)
    {
        if(player.currentHeldItem != null)
            GetComponent<Patient>().TyrCure(player.currentHeldItem.itemInfo);

        GameController.gameController.items.Remove(player.currentHeldItem.gameObject);
        Destroy(player.currentHeldItem.gameObject);
        player.currentHeldItem = null;
    }

    public void TrowAwayItem(PlayerController player) {
        Destroy(player.currentHeldItem.gameObject);
        player.currentHeldItem = null;
    } 
    void Awake()
    {
        if (GameController.gameController != null)
        {
            bool isInlist = false;

            foreach (GameObject inter in GameController.gameController.interactables)
                if (inter == gameObject)
                    isInlist = true;

            if(!isInlist)
                GameController.gameController.interactables.Add(gameObject);
        }
    }
}