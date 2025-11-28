using UnityEngine;


public class LevelStartMenu : MonoBehaviour
{
    private void Start()
    {
        OnPlayerJoin.instance.joinEvent.AddListener(DestroySelf);
    }
    private void DestroySelf()
    {
        OnPlayerJoin.instance.joinEvent.RemoveListener(DestroySelf);
        Destroy(gameObject);
    }
}
