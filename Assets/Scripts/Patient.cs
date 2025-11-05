using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VisuleseModels))]
public class Patient : MonoBehaviour
{
    internal float value = 0;
    public List<Siknes> sikneses;
    public float hellf = 100;
    [Header("Efects")]
    public ParticleSystem munyRane;
    public ParticleSystem fellMedesin;
    public Image Hellfbar;
    internal VisuleseModels visualizer;

    void Start() {
        setUp();
        visualizer = GetComponent<VisuleseModels>();

        for (int i = 0; i < sikneses.Count; i++) 
            value += sikneses[i].difecoltyAndMuny;
        PashentMan.pashents.Add(gameObject);

        if (PashentMan.pashents[0] == gameObject) DisplayMedicine();

    }

    public void setUp() {
        int nuberOfSiknese = 1;
        List<Siknes> sik = GameController.gameController.sikneses;

        for (int i = 1; i < sik.Count; i++)
            if (Random.Range(0, 5 * nuberOfSiknese) == 0)
                nuberOfSiknese++;
        Debug.Log(nuberOfSiknese);
        
        for (int i = 0; i < nuberOfSiknese; i++) {
            sikneses.Add(sik[Random.Range(0, sik.Count-1)]);
        }
    }

    public void TyrCure(ItemInfo posibolCure) {

        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);

                hellf = Mathf.Clamp(hellf + 10, 0, 100);
                
                if (sikneses.Count == 0)
                {
                    GameController.gameController.PatientHeal(this);
                    transform.DOMoveX(transform.position.x - 15, 1.5f);
                    Destroy(gameObject, 1.5f);
                }
                munyRane.Play();

                return;
            }

        fellMedesin.Play();
        hellf -= 50; // mavy difert on dirfetnt medesin ?
    }

    public void DisplayMedicine()
    {
        visualizer.clerAll();
        foreach (Siknes siknes in sikneses)
            visualizer.addItemToColdrin(siknes.cure);
    }

    void OnDestroy()
    {
        visualizer.clerAll();
        PashentMan.pashents.Remove(gameObject);
        GameController.gameController.interactables.Remove(gameObject);
        try { PashentMan.pashents[0].GetComponent<Patient>().DisplayMedicine(); } 
        catch { /*No other patients queue*/ };
    }

    private void Update()
    {
        Hellfbar.fillAmount = hellf/100; // hellf/maxhellf

        for (int i = 0; i < sikneses.Count; i++)
            hellf -= sikneses[i].hellfInpackt * Time.deltaTime;

        if (hellf < 0) { 
            GameController.gameController.PatientDie(this);
            transform.DOMoveY(transform.position.y - 5, 1.5f);
            Destroy(gameObject, 1.5f);
        }
    }
}