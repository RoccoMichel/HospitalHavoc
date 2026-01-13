using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EviermentalMan : MonoBehaviour {

    public Light[] flicker;
    List<float> orBritnes;
    public float flickerTime;
    public float flickerFrekensy;
    public AnimationCurve flickerOnOf;
    IEnumerator FlickerLite(int lite) {

        while (true)
        {
            float time = flickerTime * Random.Range(0.8f, 1.2f);
            float totolTime = time;
            while ((time -= Time.deltaTime) > -1)
            {
                yield return new WaitForEndOfFrame();
                flicker[lite].intensity = orBritnes[lite] * flickerOnOf.Evaluate(time / totolTime);
            }

            flicker[lite].intensity = orBritnes[lite];
        }
    }
    void Start() {
        for (int i = 0; i < flicker.Length; i++) {
            orBritnes.Add(flicker[i].intensity);
            StartCoroutine(FlickerLite(i));
        }
    }
   
}
