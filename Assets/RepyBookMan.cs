using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepyBookMan : MonoBehaviour
{
    public Transform[] resepyButens;
    public AvailableRecipes resepys;
    public static RepyBookMan r;

    public static bool repyBookOpen;

    void Start() {
        transform.position = Vector3.right * 100;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one / transform.lossyScale.y;
        r = this;
        GameController.gameController.Pause();

        for (int i = 0; i < resepyButens.Length; i++) {
           resepyButens[i].gameObject.SetActive(resepys.caftibols.Contains(ResepyConstruktor.resepyConstruktor.AllResepys[i]));
        }
    }

    public static void Exit() {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.lockState = CursorLockMode.Locked;
        GameController.gameController.UnPause();
        r.gameObject.SetActive(false);
    }

    public void OpenBook()
    {
        repyBookOpen = true;
        GameController.gameController.active = false;
    }
}