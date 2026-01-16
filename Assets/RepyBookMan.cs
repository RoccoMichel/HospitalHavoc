using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RepyBookMan : MonoBehaviour
{
    public Canvas canvis;
    public AvailableRecipes resepys;
    void Start() {
        Transform[] resepyButens = canvis.transform.GetComponentsInChildren<Transform>();
        for (int i = 2; i < resepyButens.Length; i++) {
           resepyButens[i].gameObject.active = resepys.caftibols.Contains(ResepyConstruktor.resepyConstruktor.AllResepys[i]);
        }
    }
}
