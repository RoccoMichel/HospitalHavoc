using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedOverride;
    private GameController gameController;
    private EventSystem eventSystem;

    public bool playTransitionOnStart = true;

    public Material transition;
    public AnimationCurve curve;
    public float transitionTimeElepsed;
    public bool forword;
    public bool isTransitening;

    bool loadScene;
    int sceneIndex = 0;
    string sceneNameToLoad = "";

    private void Start()
    {
        try { gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); }
        catch { Debug.LogWarning("Failed to get a GameController from Scene!"); }
        try { eventSystem = FindFirstObjectByType<EventSystem>().GetComponent<EventSystem>(); }
        catch { eventSystem = gameObject.AddComponent<EventSystem>().GetComponent<EventSystem>();
            gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>(); }

        eventSystem.firstSelectedGameObject = firstSelectedOverride == null ? gameObject : firstSelectedOverride;

        if(playTransitionOnStart)
            EndTransition();
    }
    public void LoadSceneByString(string sceneName)
    {
        sceneNameToLoad = sceneName;
        loadScene = true;
        StartTransition();
    }

    public void LoadSceneByIndex(int index)
    {
        sceneIndex = index;
        loadScene = true;
        StartTransition();
    }

    public void LoadSceneThis()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadScene = true;
        StartTransition();
    }

    public void ToggleSelf()
    {
        SetActive(!gameObject.activeInHierarchy);
    }

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    public void StartLevel()
    {
        gameController.SetPlayerCanJoin();
    }

    public void Pause()
    {
        gameController.Pause();
    }

    public void UnPause()
    {
        gameController.UnPause();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
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
                if (loadScene)
                {
                    if (sceneNameToLoad != "")
                        SceneManager.LoadScene(sceneNameToLoad);
                    else
                        SceneManager.LoadScene(sceneIndex);
                }

                isTransitening = false;
            }

            yield return null;
        }
    }
}