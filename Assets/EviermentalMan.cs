using System.Collections;
using UnityEngine;

public class EviermentalMan : MonoBehaviour {

    public Light[] flicker;
    public float flickerTime;
    public float flickerFrekensy;
    public AnimationCurve flickerOnOf;
    IEnumerator FlickerLite(int lite) {
        float time = flickerTime;
        float britnes = flicker[lite].intensity;

        while ((time -= Time.deltaTime) > 0) {
            yield return new WaitForEndOfFrame();
            flicker[lite].intensity = britnes * flickerOnOf.Evaluate(time / flickerTime);
        }

        while ((time += Time.deltaTime) < flickerTime) {
            yield return new WaitForEndOfFrame();
            flicker[lite].intensity = britnes * flickerOnOf.Evaluate(time / flickerTime);
        }

        flicker[lite].intensity = britnes;
    }
    private void FixedUpdate() { 
        if (Random.Range(0f, 1f) < flickerFrekensy)
            StartCoroutine(FlickerLite(Random.Range(0, flicker.Length)));
    }
}
