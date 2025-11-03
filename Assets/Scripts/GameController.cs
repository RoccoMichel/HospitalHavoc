using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public LevelDataObject levelData;
    public static GameController gameController;
    public List<GameObject> items;

    [Header("Money")]
    public bool drainMoney = true;
    public float money;

    private float time;
    
    [Space(40)] public bool debug;

    private void Awake()
    {
        gameController = this;
    }

    private void Start()
    {
        if (levelData == null) { Debug.LogError("No Level Data assigned to GameController!"); Debug.Break(); }
        money = levelData.startMoney;
        time = levelData.GameLengthSeconds;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        money -= levelData.moneyDrainRate * Time.deltaTime;

        if (money < 0) LevelFail();
        if (time < 0) LevelClear();

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


    private void LevelEnd()
    {
        drainMoney = false;
    }

    /// <summary>
    /// Level Failed by player(s)
    /// </summary>
    internal void LevelFail()
    {
        LevelEnd();
    }
    /// <summary>
    /// Successful Level completion by player(s)
    /// </summary>
    internal void LevelClear()
    {
        LevelEnd();
    }
}
