using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisuleseModels : MonoBehaviour
{
    public List<GameObject> objectInColdrin;
    public void addItemToColdrin(ItemInfo item) {
        objectInColdrin.Add(Instantiate(item.item.transform.GetChild(0).gameObject));
        objectInColdrin.Last().transform.localScale *= 0.5f;
    }
    public void clerAll() {
        for (int i = 0; i < objectInColdrin.Count; i++) {
            Destroy(objectInColdrin[i]);
        }
        objectInColdrin = new();
    }
    void Update() {
        Vector3 baseOfset = new Vector3(0, 3, -5) + transform.position;
        for (int i = 0; i < objectInColdrin.Count; i++) {
            objectInColdrin[i].transform.position = Vector3.right * 0.5f * ((i+1) - objectInColdrin.Count/2f) + baseOfset;
            objectInColdrin[i].transform.LookAt(Camera.main.transform);
        }
    }
}
