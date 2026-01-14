using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instens;
    public bool showStartMenu = true;
    public Color[] highlightColorPerPlayer;
    [Header("References")]
    [SerializeField] private TMP_Text moneyDisplay;
    [SerializeField] private TMP_Text timeDisplay;
    private GameController gameController;
    static private GameObject recipeBook;
    internal GameObject pauseMenu;

    public Slider monySlider;
    public List<RectTransform> RankRikiements;
    public RectTransform slederStart, sliderEnd;

    public void BonseMany() {

        Vector3 orgPos = moneyDisplay.rectTransform.position;
        Vector3 orgScale = moneyDisplay.rectTransform.localScale;
        float stegf = 50, time = 0.15f, stetsh = 0.9f;

        moneyDisplay.rectTransform.DOMove(orgPos + Vector3.up * stegf, time).OnComplete(() => moneyDisplay.rectTransform.DOMove(orgPos, time/2));
        moneyDisplay.rectTransform.DOScale(orgScale * stetsh, time).OnComplete(() => moneyDisplay.rectTransform.DOScale(orgScale, time/2));
    }
    void Start()
    {
        instens = this;
        int[] ranks = GameController.gameController.levelData.rankRequirements;
        monySlider.maxValue = ranks.Last();
        RankRikiements[0].position = Vector3.Lerp(slederStart.position, sliderEnd.position, (float)ranks[0] / ranks.Last());
        for (int i = 1; i < ranks.Length; i++) {
            RankRikiements.Add(Instantiate(RankRikiements[0].gameObject).GetComponent<RectTransform>());
            RankRikiements.Last().SetParent(monySlider.transform);
            RankRikiements.Last().localScale = Vector3.one;
            RankRikiements[i].position = Vector3.Lerp(slederStart.position, sliderEnd.position, (float)ranks[i] / ranks.Last());
        }


        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if ( gameController == null ) { Debug.LogError("No GameController in Scene!"); Debug.Break(); }
        gameController.canvasManager = this;

        if (showStartMenu) Instantiate((GameObject)Resources.Load("UI/Start Level Menu"), transform);
    }

    void Update()
    {
        monySlider.value = Mathf.Lerp(monySlider.value, GameController.gameController.money, Time.deltaTime * 5);

        if (timeDisplay != null) timeDisplay.text = GetTimerText(gameController.time);
        if (moneyDisplay != null) moneyDisplay.text = '$' + Mathf.Ceil(gameController.money).ToString();
    }

    public static void InstantiateRecipeBook()
    {
        if (recipeBook != null) return;
        recipeBook = Instantiate((GameObject)Resources.Load("UI/Recipe Book"), GameController.gameController.canvasManager.transform);
    }
    public static void InstantiateRecipeBook(AvailableRecipes includedRecipes)
    {
        if (recipeBook != null) return;
        recipeBook = Instantiate((GameObject)Resources.Load("UI/Recipe Book"), GameController.gameController.canvasManager.transform);
        recipeBook.GetComponent<RecipeBook>().availableRecipes = includedRecipes;
    }

    public Highlight RequestPlayerHighlight(PlayerController player)
    {
        Highlight newHighlight = Instantiate((GameObject)Resources.Load("UI/Highlight"), gameController.canvasManager.transform).GetComponent<Highlight>();
        try { newHighlight.gameObject.GetComponent<Image>().color = highlightColorPerPlayer[player.playerInt - 1]; } catch { }
        newHighlight.player = player;
        return newHighlight;
    }

    public static string GetTimerText(float time)
    {
        if (time < 0) return "0:00";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes} : {(seconds < 10 ? 0 : string.Empty)}{seconds}";
    }

    private void OnDestroy()
    {
        recipeBook = null;
    }
}
