using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public bool loopingSelection = true;
    public int playerCount = 1;
    public List<LevelData> levels = new();

    [SerializeField] private float progressIncreaseSpeed = 0.3f;
    [SerializeField] private float progressDecreaseSpeed = 0.5f;
    private float loadProgress;
    [SerializeField] private int waitingPlayers;
    private int levelIndex;

    [Header("References")]
    [SerializeField] private TMP_Text levelNameDisplay;
    [SerializeField] private Image previewDisplay;
    [SerializeField] private Image rankDisplay;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Animator boardAnimator;

    [System.Serializable]
    public struct LevelData
    {
        public string sceneName;
        public Sprite scenePreview;
    }

    public bool playTransitionOnStart = true;

    public Material transition;
    public AnimationCurve curve;
    public float transitionTimeElepsed;
    public bool forword;
    public bool isTransitening;

    private void Start()
    {
        UpdateBoard();
        GameController.gameController.active = true;

        if (playTransitionOnStart)
            EndTransition();
    }

    public void IncreaseLevelIndex(int amount)
    {
        levelIndex += amount;

        if (loopingSelection)
        {
            if (levelIndex < 0) levelIndex = levels.Count - 1;
            else if (levelIndex > levels.Count - 1) levelIndex = 0;
        }
        else levelIndex = Mathf.Clamp(levelIndex, 0, levels.Count - 1);

        UpdateBoard();
    }

    private void UpdateBoard()
    {
        boardAnimator.Play("Bounce");

        string bestRank = PlayerPrefs.GetString($"{levels[levelIndex].sceneName}_score", string.Empty);
        rankDisplay.enabled = bestRank == string.Empty ? false : true;

        if (rankDisplay.enabled) rankDisplay.sprite = Resources.Load<Sprite>("Ranks/" + bestRank);
        previewDisplay.sprite = levels[levelIndex].scenePreview;
        levelNameDisplay.text = levels[levelIndex].sceneName;
    }

    private void Update()
    {
        if (waitingPlayers < playerCount) loadProgress -= progressDecreaseSpeed * Time.deltaTime;
        else loadProgress += progressIncreaseSpeed * Time.deltaTime;
        loadProgress = Mathf.Clamp01(loadProgress);

        progressSlider.value = loadProgress;
        if (loadProgress >= 1) LoadLevel();
    }

    internal void LoadLevel()
    {
        StartTransition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waitingPlayers++;
            print(other.gameObject.name);

            playerCount = GetPlayerCount();

            print("ENTER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("EXIT1");
        waitingPlayers --;
            playerCount = GetPlayerCount();

            StartCoroutine(TriggerFix());

            print("EXIT");

        if (other.gameObject.CompareTag("Player"))
            print("wtf");
    }

    private int GetPlayerCount()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length;
    }

    public void StartTransition()
    {
        forword = true;

        transitionTimeElepsed = -1;

        isTransitening = true;

        StartCoroutine(PlayTransition());
    }

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
                SceneManager.LoadScene(levels[levelIndex].sceneName);

                isTransitening = false;
            }

            yield return null;
        }
    }

    IEnumerator TriggerFix()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
      //  waitingPlayers = 0;

        yield return new Unity.VisualScripting.WaitForNextFrameUnit();

        collider.enabled = true;

        yield break;
    }
}