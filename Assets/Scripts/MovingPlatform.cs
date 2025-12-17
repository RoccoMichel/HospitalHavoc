using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> moveTo;
    List<Vector3> pos = new List<Vector3>();

    Vector3 lastPos;

    public int currentMoveIndex;
    public float moveSpeed = 2;
    public float pointThreshold = 0.1f;

    void Update()
    {
        if(!GameController.gameController.active) return;

        int nextIndex = (currentMoveIndex + 1) % pos.Count;

        transform.position = Vector3.MoveTowards(transform.position, pos[nextIndex], moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pos[nextIndex]) < pointThreshold)
            currentMoveIndex = (currentMoveIndex + 1) % pos.Count;
    }

    void LateUpdate()
    {
        lastPos = transform.position;
    }

    void Awake()
    {
        for (int i = 0; i < moveTo.Count; i++)
        {
            if(moveTo[i] != null)
                pos.Add(moveTo[i].position);
        }
    }

    public Vector3 GetPlatformMovement()
    {
        return transform.position - lastPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < moveTo.Count; i++)
        {
            if (moveTo[i] != null)
            {
                int nextIndex = (i + 1) % moveTo.Count;
                Vector3 lineVector = (moveTo[nextIndex].position - moveTo[i].position).normalized;

                Gizmos.DrawLine(moveTo[i].position, moveTo[nextIndex].position);
            }
        }
    }
}