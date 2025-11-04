using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PashentMan : MonoBehaviour {
    public Transform[] queuePonts;
    public Gradient queueColor;
    public static List<GameObject> pashents = new();

    void OnDrawGizmos(){

        for (int i = 0; i < queuePonts.Length; i++) {
            Gizmos.color = queueColor.Evaluate((float)i/(float)queuePonts.Length);
            Gizmos.DrawSphere(queuePonts[i].position, 0.1f);
            
            if (i != 0) 
                Gizmos.DrawLine(queuePonts[i].position, queuePonts[i-1].position);
        }
    }
    void Update() {
        for (int i = 0; i < pashents.Count; i++) {
            pashents[i].transform.position = Vector3.Lerp(pashents[i].transform.position, queuePonts[i < queuePonts.Length ? queuePonts.Length : i].position, Time.deltaTime * 25);
        }
    }
}
