using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
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
        try { GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LevelStart(); } 
        catch { Debug.LogError("Failed to Start Level because Scene is missing the GameController!"); }
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
