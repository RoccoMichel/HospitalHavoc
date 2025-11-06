using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pashenst;
    public LevelDataObject levelData;
    public float money;
    public float time;
    public bool active = false;

    [Header("Backend stats")]
    public List<GameObject> items;
    public List<GameObject> interactables;
    public List<Siknes> sikneses;

    [Header("Stats")] // only public for debugging
    public int deadPatientsCount;
    public float averageCureTime = 0;
    private float latestCureTime;

    [Space(40)] public bool debug;

    public static GameController gameController;
    private InputAction pauseAction;
    internal CanvasManager canvasManager;
    private float nextPatientSpawnTime = 0;

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
        nextPatientSpawnTime = time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) debug = !debug;

        if (!active || canvasManager == null) return;

        if (pauseAction.WasPressedThisFrame() && canvasManager.pauseMenu == null)
            PauseMenu();

        time -= Time.deltaTime;
        money -= levelData.moneyDrainRate * Time.deltaTime;

        if (money < 0) LevelFail();
        if (time < 0) LevelClear();

        // Spawn a Patient "X" times a sec
        if (time < nextPatientSpawnTime) {
            Instantiate(pashenst);
            nextPatientSpawnTime = time - 1 / levelData.newPatientRate;
        }
    }

    public static void PauseMenu()
    {
        if (!gameController.active) return;
        gameController.active = false;
        gameController.canvasManager.pauseMenu = 
            Instantiate((GameObject)Resources.Load("UI/Pause Menu"), gameController.canvasManager.gameObject.transform);
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
        patient.visualizer.clerAll();
        AddMoney(patient.value);
        if (averageCureTime == 0) averageCureTime = levelData.GameLengthSeconds - time;
        else averageCureTime = (averageCureTime + latestCureTime) / 2;
        latestCureTime = time;
    }

    public void PatientDie(Patient patient)
    {
        patient.visualizer.clerAll();
        RemoveMoney(patient.value/2);
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
        if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_score", string.Empty) == string.Empty)
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_score", "F");
        Instantiate((GameObject)Resources.Load("UI/Level End Menu"), canvasManager.gameObject.transform);
    }
    /// <summary>
    /// Successful Level completion by player(s)
    /// </summary>
    internal void LevelClear()
    {
        char[] ranks = { 'F', 'D', 'C', 'B', 'A', 'S' };
        string rank = string.Empty;

        LevelEnd();

        // Calculate score
        if (averageCureTime <= 0) averageCureTime = levelData.GameLengthSeconds;
        float score = (money / averageCureTime) - (deadPatientsCount * levelData.DeadPatientScorePenalty);
        score *= 10;

        GameObject menu = Instantiate((GameObject)Resources.Load("UI/Level Clear Menu"), canvasManager.gameObject.transform);

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

        StartCoroutine(ScoreDisplay(menu, rank));

        Debug.Log($"Player(s) achieved {rank} rank with a score of: {score}!");


        // Setting High-scores |     DO NOT WRITE CODE BELOW ALWAYS ABOVE!
        string bestRank = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_score", string.Empty);
        if (bestRank == string.Empty) 
        { 
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_score", rank); 
            return; 
        }

        foreach (char rankChar in ranks)
        {
            if (rankChar == char.Parse(bestRank)) PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_score", rank);
            else if (rankChar == char.Parse(rank)) return;
        }
    }

    private IEnumerator ScoreDisplay(GameObject display, string rank)
    {
        float delayTime = 0.5f;
        TMPro.TMP_Text description = display.transform.Find("Description").GetComponent<TMPro.TMP_Text>();
        UnityEngine.UI.Image rankDisplay = display.transform.Find("Rank").GetComponent<UnityEngine.UI.Image>();

        rankDisplay.enabled = false;
        description.text = string.Empty;

        yield return new WaitForSeconds(delayTime);
        description.text += $"Money: ${Mathf.Ceil(money)}\n";

        yield return new WaitForSeconds(delayTime);
        description.text += $"Dead Patients: x{deadPatientsCount}\n";

        yield return new WaitForSeconds(delayTime);
        description.text += $"Avg. Cure Time: {CanvasManager.GetTimerText(averageCureTime)}\n\n";

        yield return new WaitForSeconds(delayTime);
        description.text += $"Rank:";

        yield return new WaitForSeconds(delayTime * 2);
        rankDisplay.enabled = true;
        rankDisplay.sprite = Resources.Load<Sprite>("Ranks/" + rank);

        yield break;
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