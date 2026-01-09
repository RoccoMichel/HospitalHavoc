using System;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent<PlayerController> onInteract;

    public bool canInteract = true, overrideInteract = true, needsEmptyHand = true, itemCanInteract, needsItem;

    public void ColdrinInteract(PlayerController player) {
        if (!GetComponent<Coldrin>().isMixing) {
            if (player.currentHeldItem != null) {
                GetComponent<Coldrin>().AddIngedent(player.currentHeldItem.itemInfo);
                GameController.gameController.items.Remove(player.currentHeldItem.gameObject);
                Destroy(player.currentHeldItem.gameObject);
                player.currentHeldItem = null;
            }
            else
                GetComponent<Coldrin>().MixIngedents();
        }
    }

    public void SpawnItem(PlayerController player) {
        GameObject spawnedItem = GetComponent<SponeItem>().sponeItem();

        player.PickUpChosenItem(spawnedItem);
    }

    public void TryCure(PlayerController player) {
        if (player.currentHeldItem == null) return;
        
        GetComponent<Patient>().TyrCure(player.currentHeldItem.itemInfo);
        GameController.gameController.items.Remove(player.currentHeldItem.gameObject);
        Destroy(player.currentHeldItem.gameObject);
        player.currentHeldItem = null;
    }

    public void TrowAwayItem(PlayerController player) {
        if (player.currentHeldItem == null) return;

        Destroy(player.currentHeldItem.gameObject);
        player.currentHeldItem = null;
        GetComponent<Animator>().SetTrigger("TroweItem");
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public void Bench(PlayerController player)
    {
        BenchController bc = GetComponent<BenchController>();

        if (player.currentHeldItem != null)
        {
            if (bc.itemsOnBench.Count < bc.itemPlaces.Count)
            {
                bc.itemsOnBench.Add(player.currentHeldItem.gameObject);
                player.currentHeldItem.transform.parent = bc.itemPlaces[bc.itemsOnBench.Count - 1];
                player.currentHeldItem = null;
            }
            else
            {
                GameObject itemToGivePlayer = bc.itemsOnBench[0];
                GameObject itemToPutOnBench = player.currentHeldItem.gameObject;

                bc.itemsOnBench[0] = itemToPutOnBench;
                itemToPutOnBench.transform.parent = bc.itemPlaces[0];
                player.currentHeldItem = null;
                player.PickUpChosenItem(itemToGivePlayer);
            }
        }
        else if (bc.itemsOnBench.Count > 0)
        {
            GameObject itemToGivePlayer = bc.itemsOnBench[bc.itemsOnBench.Count - 1];
            bc.itemsOnBench.Remove(itemToGivePlayer);

            player.PickUpChosenItem(itemToGivePlayer);
        }
    }

    void Start()
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