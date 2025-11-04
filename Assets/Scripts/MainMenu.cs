using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public float playerCount = 1;
    public List<LevelData> levels = new();

    [SerializeField] private float progressIncreaseSpeed = 0.3f;
    [SerializeField] private float progressDecreaseSpeed = 0.5f;
    private float loadProgress;
    private int waitingPlayers;
    private int levelIndex;

    [Header("References")]
    [SerializeField] private Image previewDisplay;
    [SerializeField] private Image rankDisplay;
    [SerializeField] private Slider progressSlider;

    [System.Serializable]
    public struct LevelData
    {
        public string sceneName;
        public Sprite scenePreview;
    }

    private void Start()
    {
        UpdateBoard();
        GameController.gameController.active = true;
    }

    public void IncreaseLevelIndex(int amount)
    {
        print("Level Select Interaction");
        levelIndex = Mathf.Clamp(levelIndex + amount, 0, levels.Count - 1);
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        string bestRank = PlayerPrefs.GetString($"{levels[levelIndex].sceneName}_score", string.Empty);
        rankDisplay.enabled = bestRank == string.Empty ? false : true;

        if (rankDisplay.enabled) rankDisplay.sprite = Resources.Load<Sprite>("Ranks/" + bestRank);
        previewDisplay.sprite = levels[levelIndex].scenePreview;
    }

    private void Update()
    {
        // update playerCount ?

        if (waitingPlayers < playerCount) loadProgress -= progressDecreaseSpeed * Time.deltaTime;
        else loadProgress += progressIncreaseSpeed * Time.deltaTime;
        loadProgress = Mathf.Clamp01(loadProgress);

        progressSlider.value = loadProgress;
        if (loadProgress >= 1) LoadLevel();
    }

    internal void LoadLevel()
    {
        SceneManager.LoadScene(levels[levelIndex].sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) waitingPlayers++;
        playerCount = GetPlayerCount();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) waitingPlayers--;
        playerCount = GetPlayerCount();
    }

    private int GetPlayerCount()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length;
    }
}
