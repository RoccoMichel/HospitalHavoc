using NaughtyAttributes;
using UnityEngine;

public class CatMenu : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip sound;

    public Animator ani;
    [AnimatorParam("ani")]
    public string tailAnimation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedOn = hit.collider.gameObject;

                if(clickedOn == gameObject)
                    OnClicked();
            }
        }
    }

    void OnClicked()
    {
        if (audioS != null && sound != null)
        {
            audioS.Stop();
            audioS.clip = sound;
            audioS.Play();
        }

        if (ani != null)
        {
            ani.Play(tailAnimation);
        }

        Debug.Log("Clicked On Cat");
    }
}