using UnityEngine;

public class ManeMenuCamra : MonoBehaviour
{
    public float tiltStegf;
    void Update() {
        float moseX = 1 - 2 * (float)Input.mousePosition.x / Screen.width;
        float moseY = 1 - 2 * (float)Input.mousePosition.y / Screen.height;

        transform.rotation = Quaternion.Euler(4.5f + moseY * tiltStegf, 180 - moseX * tiltStegf, 1);
    }
}
