using UnityEngine;

public class UInode : MonoBehaviour
{
    public Transform InWorldNode;
    public Camera cam;
    public Transform ConectedNode;
    public LineRenderer Line;
    void Update() {
        transform.position = cam.WorldToScreenPoint(InWorldNode.position);
        Line.SetPositions(new Vector3[]{ InWorldNode.position, ConectedNode.position});
    }
}
