using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    internal float value = 0;
    public List<Siknes> sikneses;
    
    void Start() {
        for (int i = 0; i < sikneses.Count; i++) 
            value += sikneses[i].difecoltyAndMuny;
        
        PashentMan.pashents.Add(gameObject);
    }

    public void TyrCure(ItemInfo posibolCure) {
        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);
                return;
            }
    }

    void OnDestroy() {
        PashentMan.pashents.Remove(gameObject);
    }


}
