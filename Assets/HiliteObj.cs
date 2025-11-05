using System.Collections.Generic;
using UnityEngine;

public class HiliteObj : MonoBehaviour
{
    public List<Transform> hilitedObj;
    public List<RectTransform> hilitVisols;

    void Update() {
        for (int i = 0; i < hilitVisols.Count; i++) 
            
            if (i < hilitedObj.Count) {
                hilitVisols[i].gameObject.SetActive(true);
                hilitVisols[i].position = Camera.main.WorldToScreenPoint(hilitedObj[i].position);
            }
            else hilitVisols[i].gameObject.SetActive(false);
    }
}
