using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuitField : MonoBehaviour
{
    public int playerCount = 1;
    [SerializeField] private float progressIncreaseSpeed = 0.3f;
    [SerializeField] private float progressDecreaseSpeed = 0.5f;
    private float loadProgress;
    [SerializeField] private int waitingPlayers;

    [Header("References")]
    [SerializeField] private Slider progressSlider;

    private void Update()
    {
        if (waitingPlayers < playerCount) loadProgress -= progressDecreaseSpeed * Time.deltaTime;
        else loadProgress += progressIncreaseSpeed * Time.deltaTime;
        loadProgress = Mathf.Clamp01(loadProgress);

        progressSlider.value = loadProgress;
        if (loadProgress >= 1) 
        {
            Debug.Log("Players Quit the Application");
            Application.Quit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waitingPlayers++;
            playerCount = GetPlayerCount();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waitingPlayers--;
            playerCount = GetPlayerCount();

            StartCoroutine(TriggerFix());
        }
    }

    private int GetPlayerCount()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length;
    }

    IEnumerator TriggerFix()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
        waitingPlayers = 0;

        yield return new Unity.VisualScripting.WaitForNextFrameUnit();

        collider.enabled = true;

        yield break;
    }
}
