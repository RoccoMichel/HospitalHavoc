using UnityEngine;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    public UnityEvent<PlayerController> onInteract;

    public void ColdrinInteract(PlayerController player)
    {
        if (player.currentHeldItem != null)
            Coldrin.inctanse.AddIngedent(player.currentHeldItem.itemInfo);
        else
            Coldrin.inctanse.MixIngedents();
    }

    public void SpawnItem()
    {
        GetComponent<SponeItem>().sponeItem();
    }
}