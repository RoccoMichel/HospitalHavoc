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
    public MeshRenderer[] Ilneses;
    public GameObject Spitshbubol;
    public int qerentQupont;
    //internal VisuleseModels visualizer;
    bool ded = false;
    bool haseRitshFrontofQue = false;
    void Start() {
        setUp();
        //visualizer = GetComponent<VisuleseModels>();

        for (int i = 0; i < sikneses.Count; i++) 
            value += sikneses[i].difecoltyAndMuny;
        PashentMan.pashents.Add(gameObject);

        if (PashentMan.pashents[0] == gameObject) 
            DisplayMedicine();

    }

    public void setUp() {
        int nuberOfSiknese = 1;
        List<Siknes> sik = GameController.gameController.sikneses;
        qerentQupont = PashentMan.instance.queuePonts.Length-1;
        for (int i = 1; i < sik.Count; i++)
            if (Random.Range(0, 5 * nuberOfSiknese) == 0)
                nuberOfSiknese++;
        
        for (int i = 0; i < nuberOfSiknese; i++) {
            sikneses.Add(sik[Random.Range(0, sik.Count)]);
        }

        for (int i = 0; i < Ilneses.Length; i++)
            Ilneses[i].material = Instantiate(Ilneses[i].material);
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
            StartCoroutine(GoToPont(quePonts[i].position, quePonts[i-1].position, (PashentMan.instance.whakeTime / quePonts.Length) *0.5f));
            yield return new WaitForSeconds((PashentMan.instance.whakeTime / quePonts.Length)*0.5f);
        }
      
    }
    public void TyrCure(ItemInfo posibolCure) {

        for (int i = 0; i < sikneses.Count; i++) 
            if (sikneses[i].cure == posibolCure) { 
                sikneses.RemoveAt(i);

                hellf = Mathf.Clamp(hellf + 10, 0, 100);
                DisplayMedicine();

                if (sikneses.Count == 0)
                {
                    Spitshbubol.SetActive(false);
                    PashentMan.pashents.Remove(gameObject);
                    GameController.gameController.interactables.Remove(gameObject);
                    GameController.gameController.PatientHeal(this);
                    Destroy(gameObject, PashentMan.instance.whakeTime);
                    StartCoroutine(RunAway());
                    try { PashentMan.pashents[0].GetComponent<Patient>().DisplayMedicine(); }
                    catch { /*No other patients queue*/ };
                }
                munyRane.Play();

                return;
            }

        fellMedesin.Play();
        hellf -= 50; // mavy difert on dirfetnt medesin ?
    }

    public void DisplayMedicine()
    {
        Spitshbubol.SetActive(true);
        for (int i = 0; i < Ilneses.Length; i++) {
            if (sikneses.Count > i) Ilneses[i].material.SetTexture("_BaseMap", sikneses[i].pitsher);
            else Ilneses[i].material.SetColor("_BaseColor", Color.clear);
        }

        //visualizer.clerAll();
        //foreach (Siknes siknes in sikneses)
        //    visualizer.addItemToColdrin(siknes.cure);
    }

    void OnDestroy()
    {
      //  visualizer.clerAll();
        PashentMan.pashents.Remove(gameObject);
        GameController.gameController.interactables.Remove(gameObject);
       
    }

    void Update()
    {
        qerentQupont = PashentMan.pashents.IndexOf(gameObject);
        Hellfbar.fillAmount = hellf/100; // hellf/maxhellf

        //if (PashentMan.pashents.Count <= qerentQupont && Vector3.Distance(transform.position, PashentMan.instance.queuePonts[qerentQupont].position) < 1f)
        //    qerentQupont--;

        if (qerentQupont == 0) 
            haseRitshFrontofQue = true;

        if (!ded && !haseRitshFrontofQue) 
            transform.position = Vector3.Lerp(transform.position, PashentMan.instance.queuePonts[qerentQupont].position, Time.deltaTime * 60 / PashentMan.instance.whakeTime);


        if (!GameController.gameController.active) return; // Freeze when game is Paused

        for (int i = 0; i < sikneses.Count; i++)
            hellf -= sikneses[i].hellfInpackt * Time.deltaTime;

        if (hellf < 0) {
            if (!ded) GameController.gameController.PatientDie(this);
            transform.DOMoveY(transform.position.y - 5, 1.5f);
            transform.DORotate(new Vector3(1, 0, 0), 1.5f);
            Destroy(gameObject, 1.5f);
            ded = true;
        }
    }

    private void LateUpdate()
    {
         if (ded || sikneses.Count == 0) Spitshbubol.SetActive(false);
    }
}