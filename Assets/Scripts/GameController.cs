using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class GameController : MonoBehaviour
{
    public LevelDataObject levelData;
    public float money;
    public float time;
    private bool active = false;

    /*[HideInInspector]*/ public List<GameObject> items;
    /*[HideInInspector]*/ public List<GameObject> interactables;

    [Header("Stats")] // only public for debugging
    public int deadPatientsCount;
    public float averageCureTime = 0;
    private float latestCureTime;

    [Space(40)] public bool debug;

    private InputAction pauseAction;
    public static GameController gameController;
    internal CanvasManager canvasManager;


    private void Awake()
    {
        gameController = this;
        active = false;
    }

    private void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
        if (levelData == null) { Debug.LogError("No Level Data assigned to GameController!"); Debug.Break(); }

        money = levelData.startMoney;
        time = levelData.GameLengthSeconds;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) debug = !debug;

        if (!active) return;

        if (pauseAction.WasPressedThisFrame() && canvasManager.pauseMenu == null)
        {
            Pause();
            canvasManager.pauseMenu = 
                Instantiate((GameObject)Resources.Load("UI/Pause Menu"), canvasManager.gameObject.transform);
        }

        time -= Time.deltaTime;
        money -= levelData.moneyDrainRate * Time.deltaTime;

        if (money < 0) LevelFail();
        if (time < 0) LevelClear();

    }

    public void AddMoney(float amount)
    {
        money += amount;
    }

    public void RemoveMoney(float amount)
    {
        money -= amount;
    }

    public void PatientHeal(Patient patient)
    {
        AddMoney(patient.value);
        if (averageCureTime == 0) averageCureTime = levelData.GameLengthSeconds - time;
        else averageCureTime = (averageCureTime + latestCureTime) / 2;
        latestCureTime = time;
    }

    public void PatientDie(Patient patient)
    {
        RemoveMoney(patient.value);
        deadPatientsCount++;
    }

    public void LevelStart()
    {
        active = true;
    }
    private void LevelEnd()
    {
        active = false;
    }

    /// <summary>
    /// Level Failed by player(s)
    /// </summary>
    internal void LevelFail()
    {
        LevelEnd();

        Instantiate((GameObject)Resources.Load("UI/Level End Menu"), canvasManager.gameObject.transform);
    }
    /// <summary>
    /// Successful Level completion by player(s)
    /// </summary>
    internal void LevelClear()
    {
        char[] ranks = { 'D', 'C', 'B', 'A', 'S' };
        string rank = string.Empty;

        LevelEnd();

        // Calculate score
        if (averageCureTime <= 0) averageCureTime = 1;
        float score = (money / averageCureTime) - (deadPatientsCount * levelData.DeadPatientScorePenalty);

        Instantiate((GameObject)Resources.Load("UI/Level Clear Menu"), canvasManager.gameObject.transform);

        if (ranks.Length != levelData.rankRequirements.Length) {
            Debug.LogError("Different amounts of Ranks in GameController and LevelData!\t"
                + $"GameController: {ranks.Length} Elements | LevelData: {levelData.rankRequirements.Length} Elements");
            return;
        }

        for (int i = 0; i < levelData.rankRequirements.Length; i++)
        {
            rank = ranks[i].ToString();
            if (score < levelData.rankRequirements[i]) break;
        }
        Debug.Log($"Player(s) achieved {rank} rank with a score of: {score}!");
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_score", "A");
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

    public void Pause()
    {
        active = false;
    }

    public void UnPause()
    {
        active = true;
    }

    private void Reset()
    {
        gameObject.tag = "GameController";
    }

    void OnValidate()
    {
        gameController = this;
    }
}