using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepyBookMan : MonoBehaviour
{
    public Transform[] resepyButens;
    public AvailableRecipes resepys;
    public static RepyBookMan r;
    void Start() {
        transform.position = Vector3.right * 100;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one / transform.lossyScale.y;
        r = this;
        for (int i = 0; i < resepyButens.Length; i++) {
           resepyButens[i].gameObject.SetActive(resepys.caftibols.Contains(ResepyConstruktor.resepyConstruktor.AllResepys[i]));
        }
    }

    private void Update() {
        Cursor.lockState = CursorLockMode.Confined;
        if (GameController.gameController.active == false) Exit();
    }

    public static void Exit() {
        Cursor.lockState = CursorLockMode.Locked;
        r.gameObject.SetActive(false);
    }
}
