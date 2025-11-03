using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        try { gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); }
        catch { Debug.LogError("Failed to Start Level because Scene is missing the GameController!"); }
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
        gameController.LevelStart();
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
