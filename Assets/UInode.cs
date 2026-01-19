using UnityEngine;

public class UInode : MonoBehaviour
{
    public Transform InWorldNode;
    public Camera cam;
    public Transform ConectedNode;
    public LineRenderer Line;

    Vector3 nodPos() {
        return InWorldNode.position + Vector3.forward;
    }
    void Update() {
        transform.position = nodPos();
        Line.SetPositions(new Vector3[]{ nodPos(), ConectedNode.position});
    }
}
