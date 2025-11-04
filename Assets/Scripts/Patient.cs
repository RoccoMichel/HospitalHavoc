using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    internal float value = 0;
    public List<Siknes> sikneses;
    public float hellf = 100;
    void Awake() {
        for (int i = 0; i < sikneses.Count; i++) 
            value += sikneses[i].difecoltyAndMuny;
        setUp();
        PashentMan.pashents.Add(gameObject);
    }
    public void setUp() {
        int nuberOfSiknese = 1;
        List<Siknes> sik = GameController.gameController.sikneses;

        for (int i = 1; i < sik.Count; i++)
            if (Random.Range(0, 5 * nuberOfSiknese) == 0)
                nuberOfSiknese++;

        for (int i = 0; i < nuberOfSiknese; i++) {
            sikneses.Add(sik[Random.Range(0, sik.Count-1)]);
        }
    }
    public void TyrCure(ItemInfo posibolCure) {
        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);
                return;
            }

        if (sikneses.Count == 0)
            GameController.gameController.PatientHeal(this);
    }

    void OnDestroy() {
        GameController.gameController.PatientDie(this);
        PashentMan.pashents.Remove(gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < sikneses.Count; i++)
            hellf -= sikneses[i].hellfInpackt * Time.deltaTime;
    }
}
