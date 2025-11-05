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
                Vector3 Npos = Camera.main.WorldToViewportPoint(hilitedObj[i].position);
                hilitVisols[i].localPosition = new Vector3(Npos.x * Screen.width/2, Npos.y * Screen.height/2, 0);
            }
            else hilitVisols[i].gameObject.SetActive(false);
    }
}
