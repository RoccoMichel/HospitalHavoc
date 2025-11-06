using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedOverride;
    private GameController gameController;
    private EventSystem eventSystem;

    private void Start()
    {
        try { gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); }
        catch { Debug.LogWarning("Failed to get a GameController from Scene!"); }
        try { eventSystem = FindFirstObjectByType<EventSystem>().GetComponent<EventSystem>(); }
        catch { eventSystem = gameObject.AddComponent<EventSystem>().GetComponent<EventSystem>();
            gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>(); }

        eventSystem.firstSelectedGameObject = firstSelectedOverride == null ? gameObject : firstSelectedOverride;
    }
    public void LoadSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneThis()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}