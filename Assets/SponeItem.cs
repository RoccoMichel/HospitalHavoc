using UnityEngine;

public class SponeItem : MonoBehaviour {
    public ItemInfo spone;
    // spons the coret item and returns it
    public GameObject sponeItem() {
        return Instantiate(spone.item, transform.position, transform.rotation);
    }
}
