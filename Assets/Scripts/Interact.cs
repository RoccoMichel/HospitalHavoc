using System;
using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent<PlayerController> onInteract;

    public bool needsEmptyHand = true;

    public void ColdrinInteract(PlayerController player)
    {
        if (player.currentHeldItem != null)
        {
            Coldrin.inctanse.AddIngedent(player.currentHeldItem.itemInfo);
            GameController.gameController.items.Remove(player.currentHeldItem.gameObject);
            Destroy(player.currentHeldItem.gameObject);
        }
        else Coldrin.inctanse.MixIngedents();
    }

    public void SpawnItem()
    {
        Debug.Log("Spone item");
        GetComponent<SponeItem>().sponeItem();
    }

    public void TryCure(PlayerController player)
    {
        if(player.currentHeldItem != null)
            GetComponent<Patient>().TyrCure(player.currentHeldItem.itemInfo);
    }

    void OnValidate()
    {
        if(GameController.gameController != null)
            GameController.gameController.interactables.Add(gameObject);
    }
}