using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisuleseModels : MonoBehaviour
{
    public float scaleFactor = 0.5f;
    public Vector3 offset = new (0, 1, 10);
    public List<GameObject> objectInColdrin = new();
    public void addItemToColdrin(ItemInfo item) {
        objectInColdrin.Add(Instantiate(item.item.transform.GetChild(0).gameObject));
        objectInColdrin.Last().transform.localScale *= scaleFactor;
    }
    public void clerAll() {
        for (int i = 0; i < objectInColdrin.Count; i++) {
            Destroy(objectInColdrin[i]);
        }
        objectInColdrin = new();
    }
    void Update() {
        Vector3 finalOffset = offset + transform.position;
        for (int i = 0; i < objectInColdrin.Count; i++) {
            float bobelStref = 0.15f;
            float bobleSped = 4;
            objectInColdrin[i].transform.position = Vector3.right * 0.5f * ((i+1) - objectInColdrin.Count/2f) + finalOffset 
                + Vector3.up * Mathf.Sin(i + Time.time * bobleSped) * bobelStref; // Animate the items
            objectInColdrin[i].transform.LookAt(Camera.main.transform);
        }


    }
}
