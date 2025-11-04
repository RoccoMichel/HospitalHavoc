using UnityEngine;

public class SponeItem : MonoBehaviour {
    public ItemInfo spone;
    public Transform sideObj;
    // spons the coret item and returns it
    public GameObject sponeItem() {
        return Instantiate(spone.item, transform.position, transform.rotation);
    }

    void Start() {
        GameObject item = Instantiate(spone.item.transform.GetChild(0).gameObject);
        item.transform.parent = sideObj.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale *= 0.5f;
        item.transform.localRotation = Quaternion.Euler(0,0,0);

    }
}
