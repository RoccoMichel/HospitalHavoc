using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    internal float value = 0;
    public List<Siknes> sikneses;
    public float hellf = 100;
    public ParticleSystem munyRane;
    void Awake() {
        setUp();
        for (int i = 0; i < sikneses.Count; i++) 
            value += sikneses[i].difecoltyAndMuny;
        PashentMan.pashents.Add(gameObject);
    }
    public void setUp() {
        int nuberOfSiknese = 1;
        List<Siknes> sik = GameController.gameController.sikneses;

        for (int i = 1; i < sik.Count; i++)
            if (Random.Range(0, 5 * nuberOfSiknese) == 0)
                nuberOfSiknese++;

        for (int i = 0; i <= nuberOfSiknese; i++) {
            sikneses.Add(sik[Random.Range(0, sik.Count-1)]);
        }
    }
    public void TyrCure(ItemInfo posibolCure) {
        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);
                if (sikneses.Count == 1)
                {
                    GameController.gameController.PatientHeal(this);
                    Destroy(gameObject, 0.5f);
                }
                munyRane.Play();

                return;
            }
    }

    void OnDestroy() {
        PashentMan.pashents.Remove(gameObject);
    }

    private void Update()
    {
        for (int i = 0; i < sikneses.Count; i++)
            hellf -= sikneses[i].hellfInpackt * Time.deltaTime;

        if (hellf < 0) { 
            GameController.gameController.PatientDie(this);
            Destroy(gameObject);
        }
    }
}
