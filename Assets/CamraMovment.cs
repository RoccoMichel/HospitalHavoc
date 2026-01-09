using System.Collections.Generic;
using UnityEngine;

public class CamraMovment : MonoBehaviour {
    public float turnStregf;
    public Transform lutAt, CeterOfmap;
    Vector3 camOridenPos = new();
    void Start() {
        camOridenPos = transform.position;
    }
    void Update() {
        lutAt.position = CeterOfmap.position;
        List<PlayerController> pl = OnPlayerJoin.instance.players;
        for (int i = 0; i < OnPlayerJoin.instance.players.Count; i++) {
            lutAt.position = Vector3.Lerp(lutAt.position, pl[i].transform.position, turnStregf);
        }

       // transform.position = Vector3.LerpUnclamped(camOridenPos, new Vector3(camOridenPos.x, camOridenPos.y, Mathf.Min(0, lutAt.position.z - camOridenPos.z)), 0.15f);
        transform.LookAt(lutAt);
    }
}
