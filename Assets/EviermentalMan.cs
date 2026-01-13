using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class EviermentalMan : MonoBehaviour {

    public Light[] flicker;
    List<float> orBritnes = new();
    public float flickerTime;
    public AnimationCurve flickerOnOf;
    IEnumerator FlickerLite(int lite) {

        while (true) {
            float time = flickerTime * Random.Range(0.5f, 1.5f);
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

    public List<GameObject> prefabsToCheckFor;

    [Button]
    void AddAllLights()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        List<Light> foundLights = new();

        foreach (GameObject obj in allObjects)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(obj))
            {
                GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);

                foreach (var prefab in prefabsToCheckFor)
                {
                    if (sourcePrefab == prefab)
                    {
                        foundLights.Add(obj.GetComponentInChildren<Light>());
                    }
                }
            }
        }

        List<Light> newArray = flicker.ToList();

        foreach (var obj in foundLights)
            newArray.Add(obj);

        flicker = newArray.ToArray();
    }
}