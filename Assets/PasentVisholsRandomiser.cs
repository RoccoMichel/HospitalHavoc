using System.Collections.Generic;
using UnityEngine;

public class PasentVisholsRandomiser : MonoBehaviour
{
    public List<Sprite> Skin, UperBody, Fase, Haer, Pants, Shose, Extras;
    public List<SpriteRenderer> Renderers;
    Sprite GetRan(List<Sprite> sprits) => 
        sprits[Random.Range(0, sprits.Count)];
    public void RandomiseAperns() {
        Renderers[0].sprite = GetRan(Skin);
        Renderers[1].sprite = GetRan(UperBody);
        Renderers[2].sprite = GetRan(Fase);
        Renderers[3].sprite = GetRan(Haer);
        Renderers[4].sprite = GetRan(Pants);
        Renderers[5].sprite = GetRan(Shose);
        if (Random.Range(0, 100) < 6) 
            Renderers[6].sprite = GetRan(Extras); // 5% 
    }
    private void Start()
    {
        RandomiseAperns();
    }
}
