using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject pashenst;
    [Expandable]
    public LevelDataObject levelData;
    public float money;
    public float time;
    public bool active = false;

    public Material transition;
    public AnimationCurve curve;
    public float transitionTimeElepsed;
    public bool forword;
    public bool isTransitening;

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

    public bool spawnPatients = true;

    private void Awake()
    {
        GameSaveController.Initialize();

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

        if(!spawnPatients)
            SetPlayerCanJoin();
    }

    float scors() {
        LevelData data = GameSaveController.LoadData(levelData.data.level);

        return data.score;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) debug = !debug;

        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.Confined;

        if (!active || canvasManager == null) return;

        if (pauseAction.WasPressedThisFrame() && canvasManager.pauseMenu == null)
            PauseMenu();

        time -= Time.deltaTime;
        // money -= levelData.moneyDrainRate * Time.deltaTime;

        if (money < 0) LevelFail();
        if (time < 0) LevelClear();

        if (spawnPatients)
        {
            // Spawn a Patient "X" times a sec
            if (time < nextPatientSpawnTime || PashentMan.pashents.Count == 0)
            {
                Instantiate(pashenst);
                nextPatientSpawnTime = time - 1 / levelData.newPatientRate;
            }
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
        AddMoney(patient.value);
        if (averageCureTime == 0) averageCureTime = levelData.GameLengthSeconds - time;
        else averageCureTime = (averageCureTime + latestCureTime) / 2;
        latestCureTime = time;
    }

    public void PatientDie(Patient patient)
    {
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

    public void SetPlayerCanJoin()
    {
        OnPlayerJoin.instance.GetComponent<PlayerInputManager>().EnableJoining();
    }

    /// <summary>
    /// Level Failed by player(s)
    /// </summary>
    internal void LevelFail()
    {
        LevelEnd();
        if (levelData.data.rank == string.Empty || levelData.data.rank == "")
        {
            levelData.data.rank = "F";

            int playerCount = OnPlayerJoin.instance.players.Count + 1;
            levelData.data.players = playerCount;

            GameSaveController.Save(levelData.data);
        }
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
        if (averageCureTime <= 0) averageCureTime = levelData.GameLengthSeconds;
        float score = money;

        levelData.data.score = score;

        // Show result to player(s)
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


        // Save new high-scores |    /!\   >  /!\   >  /!\   >  /!\   >  /!\   >  /!\   >  DO NOT WRITE CODE BELOW ALWAYS ABOVE  /!\
        string bestRank = levelData.data.rank;
        if (bestRank == string.Empty || bestRank == "F") 
        { 
            levelData.data.rank = rank;
            return; 
        }

        foreach (char rankChar in ranks)
        {
            if (rankChar == char.Parse(bestRank))
            {
                levelData.data.rank = rank;

                int playerCount = OnPlayerJoin.instance.players.Count + 1;
                levelData.data.players = playerCount;
            }
            else if (rankChar == char.Parse(rank)) return;
        }

        GameSaveController.Save(levelData.data);
    }

    [Button]
    void testSave()
    {
        GameSaveController.Save(levelData.data);
    }

    [Button]
    public void StartTransition()
    {
        forword = true;

        transitionTimeElepsed = -1;

        isTransitening = true;

        StartCoroutine(PlayTransition());
    }

    [Button]
    public void EndTransition()
    {
        forword = false;

        transitionTimeElepsed = 1;

        isTransitening = true;

        StartCoroutine(PlayTransition());
    }

    IEnumerator PlayTransition()
    {
        while (isTransitening)
        {
            transitionTimeElepsed += Time.unscaledDeltaTime * (forword ? 1 : -1);

            transition.SetFloat("_Size", curve.Evaluate(transitionTimeElepsed) * 100);

            if (Mathf.Abs(transitionTimeElepsed) > 1)
            {
                isTransitening = false;
            }

            yield return null;
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

        // Text
        GUI.Label(new Rect(10, 10, 100, 20), $"ms per frame: {System.Decimal.Round((decimal)(Time.deltaTime * 1000), 2)} ");

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
        gameObject.name = "Game Controller";
        gameObject.transform.position = Vector3.zero;
    }

    void OnValidate()
    {
        gameController = this;
    }
}