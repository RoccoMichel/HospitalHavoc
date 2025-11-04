using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text moneyDisplay;
    [SerializeField] private TMP_Text timeDisplay;
    private GameController gameController;
    internal GameObject pauseMenu;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if ( gameController == null ) { Debug.LogError("No GameController in Scene!"); Debug.Break(); }
        gameController.canvasManager = this;

        Instantiate((GameObject)Resources.Load("UI/Start Level Menu"), transform);
    }

    void Update()
    {
        timeDisplay.text = GetTimerText(gameController.time);
        moneyDisplay.text = '$' + Mathf.Ceil(gameController.money).ToString();
    }

    public static string GetTimerText(float time)
    {
        if (time < 0) return "0:00";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes} : {(seconds < 10 ? 0 : string.Empty)}{seconds}";
    }
}
