using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public int tutorialStage = 0;
    public TutorialElement[] tutorial;
    [SerializeField] TMP_Text explanationDisplay;
    private GameObject indicator;

    [System.Serializable]
    public struct TutorialElement
    {
        public Transform arrowLocation;
        [TextArea] public string explanation;
    }

    private void Start() => RefreshTutorial();

    public void AdvanceTutorial(int fromStage)
    {
        if (tutorialStage != fromStage) return;

        tutorialStage++;
        RefreshTutorial();
    }

    public void RefreshTutorial()
    {
        if (indicator == null) indicator = Instantiate((GameObject)Resources.Load("Indicator"));
        indicator.transform.position = tutorial[tutorialStage].arrowLocation.position;
        explanationDisplay.text = tutorial[tutorialStage].explanation;
    }

    public bool CanAdvance(int currentStage) { return currentStage == tutorialStage; }

    public void SetGameTime(float newTime)
    {
        GameController.gameController.time = newTime;
    }
}