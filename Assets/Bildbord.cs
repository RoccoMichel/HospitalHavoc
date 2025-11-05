using UnityEngine;

public class Bildbord : MonoBehaviour
{
  
    void Update() {
        transform.LookAt(Camera.main.transform);
    }
}
