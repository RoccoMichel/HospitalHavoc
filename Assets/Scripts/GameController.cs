using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool debug;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) debug = !debug;
    }

    private void OnGUI()
    {
        if (!debug) return;

        GUIStyle style = new()
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
        };
        // Text
        GUI.Label(new Rect(10, 10, 100, 20), $"ms per frame: {System.Decimal.Round((decimal)(Time.deltaTime * 1000), 2)} ", style);

        // Buttons
        if (GUI.Button(new Rect(10, 40, 100, 20), "Reload")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (GUI.Button(new Rect(10, 70, 100, 20), "Exit")) Application.Quit(); ;
    }
}
