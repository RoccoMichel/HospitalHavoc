using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int relevantStage = 1;
    [SerializeField] private TutorialManager tutorialManager;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        tutorialManager.AdvanceTutorial(relevantStage);
    }
}
