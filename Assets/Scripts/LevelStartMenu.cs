using UnityEngine;


public class LevelStartMenu : MonoBehaviour
{
    private void Start()
    {
        OnPlayerJoin.instance.joinEvent.AddListener(DestroySelf);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
        OnPlayerJoin.instance.joinEvent.RemoveListener(DestroySelf);
    }
}