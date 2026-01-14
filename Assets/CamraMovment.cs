using System.Collections.Generic;
using UnityEngine;

public class CamraMovment : MonoBehaviour {
    public float turnStregf;
    public Transform lutAt, CeterOfmap;
    public float MaxMovent = 5;

    Vector3 camOridenPos = new();
    void Start() {
        camOridenPos = transform.position;
    }
    void Update() {

        List<PlayerController> pl = OnPlayerJoin.instance.players;

        Vector3 plPos = Vector3.zero;
        for (int i = 0; i < OnPlayerJoin.instance.players.Count; i++)
        {
            plPos += pl[i].transform.position;
        }
        plPos *= 1f / OnPlayerJoin.instance.players.Count;

        //Vector3 dis = new Vector3(plPos.x, camOridenPos.y, plPos.z) - camOridenPos;
        //if (dis.magnitude > radius) transform.position = plPos - dis.normalized * radius;
        //else transform.position = camOridenPos;
        //transform.position = new Vector3(transform.position.x, camOridenPos.y, transform.position.z);


        if (OnPlayerJoin.instance.players.Count > 0)
        {
            float zOfset = Mathf.Min(MaxMovent, Mathf.Max(0, plPos.z - CeterOfmap.position.z));
            lutAt.position = CeterOfmap.position + Vector3.forward * zOfset;

            transform.position = Vector3.Lerp(transform.position, camOridenPos + Vector3.forward * zOfset, 10 * Time.deltaTime);
            for (int i = 0; i < OnPlayerJoin.instance.players.Count; i++)
            {
                lutAt.position = Vector3.Lerp(lutAt.position, pl[i].transform.position, turnStregf);
            }
            transform.LookAt(lutAt);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(camOridenPos, MaxMovent);
        Gizmos.DrawWireSphere(transform.position, MaxMovent);
    }
}
