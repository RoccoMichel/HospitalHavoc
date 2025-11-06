using DG.Tweening;
using System.Collections;
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
    bool ded = false;

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
            sikneses.Add(sik[Random.Range(0, sik.Count)]);
        }
    }
    IEnumerator GoToPont(Vector3 orgPos, Vector3 pos, float time) { 
        for (float t = time; t > 0; t -= Time.deltaTime) {
            transform.position = Vector3.Lerp(orgPos, pos, t/time);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator RunAway() {
        Transform[] quePonts = PashentMan.instance.queuePonts;
        for (int i = 1; i < quePonts.Length; i++) {
            StartCoroutine(GoToPont(quePonts[i].position, quePonts[i-1].position, 1.5f / quePonts.Length));
            yield return new WaitForSeconds(1.5f/ quePonts.Length);
        }
      
    }
    public void TyrCure(ItemInfo posibolCure) {

        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);

                hellf = Mathf.Clamp(hellf + 10, 0, 100);
                
                if (sikneses.Count == 0)
                {
                    PashentMan.pashents.Remove(gameObject);
                    GameController.gameController.interactables.Remove(gameObject);
                    GameController.gameController.PatientHeal(this);
                    Destroy(gameObject, 1.5f);
                    StartCoroutine(RunAway());
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
            if(!ded) GameController.gameController.PatientDie(this);
            transform.DOMoveY(transform.position.y - 5, 1.5f);
            transform.DORotate(new Vector3(1, 0, 0), 1.5f);
            Destroy(gameObject, 1.5f);
            ded = true;
        }
    }
}