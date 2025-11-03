using System.Collections.Generic;
using UnityEngine;

public class ResepyBok : MonoBehaviour
{
    public List<ItemInfo> AllCraftebolItems;
    public static int curentResepy;
    void Update() {
        Transform cam = Camera.main.transform;
        transform.position = Vector3.Lerp(transform.position, cam.position + cam.forward*5, Time.deltaTime * 10);
        transform.LookAt(cam);
    }

    public static void nextResepy() {
        curentResepy++;
        upadeVisholes();
    }
    public static void upadeVisholes() { 
        
    }
}
