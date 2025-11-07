using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public int relevantStage = 1;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private UnityEvent events;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!tutorialManager.CanAdvance(relevantStage)) return;

        tutorialManager.AdvanceTutorial(relevantStage);
        events.Invoke();
    }
}
