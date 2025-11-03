using NaughtyAttributes;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Expandable]
    public ItemInfo itemInfo;
    void Update() {
        float sped = 100;
        if (transform.parent != null) {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position + transform.parent.forward, Time.deltaTime * sped);
        }
    }
}