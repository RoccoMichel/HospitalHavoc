using System.Collections.Generic;
using UnityEngine;

public class SponeItem : MonoBehaviour {
    public ItemInfo spone;
    public Transform sideObj;
    public Transform ItemShocse;
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

        GameObject item2 = Instantiate(spone.item.transform.GetChild(0).gameObject, ItemShocse.position, ItemShocse.rotation);
        item2.transform.SetParent(ItemShocse);
    }

    void Update() {
        Vector3 closetPos = Vector3.one * 1000;
        List<PlayerController> pl = OnPlayerJoin.instance.players;
        for (int i = 0; i < OnPlayerJoin.instance.players.Count; i++)
            if (Vector3.Distance(closetPos, transform.position) > Vector3.Distance(pl[i].transform.position, transform.position))
            closetPos = pl[i].transform.position;

        float efect = 0.1f;
        ItemShocse.localScale = Vector3.one * Mathf.Lerp(2, 0, Mathf.Pow(Mathf.Clamp01(Vector3.Distance(closetPos, transform.position) * efect),2));
    }
}
