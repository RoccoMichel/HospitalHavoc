using UnityEngine;

public class Drawing
{
    private static Texture2D _lineTex;

    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    {
        Matrix4x4 matrix = GUI.matrix;

        if (!_lineTex)
        {
            _lineTex = new Texture2D(1, 1);
            _lineTex.SetPixel(0, 0, Color.white);
            _lineTex.Apply();
        }

        Color savedColor = GUI.color;
        GUI.color = color;

        float angle = Vector3.Angle(pointB - pointA, Vector2.right);

        if (pointA.y > pointB.y)
            angle = -angle;

        float length = (pointB - pointA).magnitude;

        GUIUtility.RotateAroundPivot(angle, pointA);
        GUI.DrawTexture(new Rect(pointA.x, pointA.y - (width / 2), length, width), _lineTex);
        GUI.matrix = matrix;
        GUI.color = savedColor;
    }
}
