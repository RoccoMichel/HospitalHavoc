using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public void PlayAni(GameObject currentModel)
    {
        Animator ani = currentModel.GetComponent<Animator>();
        if (ani != null)
            ani.speed = 1;
    }

    public void StopAni(GameObject currentModel)
    {
        Animator ani = currentModel.GetComponent<Animator>();

        if (ani != null)
        {
            ani.Play("walk", 0, 0);
            ani.speed = 0;
        }
    }
}