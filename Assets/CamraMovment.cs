using System.Collections.Generic;
using UnityEngine;

public class CamraMovment : MonoBehaviour {
    public float turnStregf;
    public Transform lutAt, CeterOfmap;

    void Update() {
        lutAt.position = CeterOfmap.position;
        List<PlayerController> pl = OnPlayerJoin.intance.players;
        for (int i = 0; i < OnPlayerJoin.intance.players.Count; i++) {
            lutAt.position = Vector3.Lerp(lutAt.position, pl[i].transform.position, turnStregf);
        }

        transform.LookAt(lutAt);
    }
}
