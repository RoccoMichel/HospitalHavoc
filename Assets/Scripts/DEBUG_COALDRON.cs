using UnityEngine;

public class DEBUG_COALDRON : MonoBehaviour
{
    public Coldrin troubleMaker;
    public Interact interact;
    public GameController gameController;

    private bool inList;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 100, 500, 20), $"GO \'{troubleMaker.name}\' DEBUG INFO:");
        GUI.Label(new Rect(10, 120, 500, 20), $"({gameObject.activeInHierarchy}) active");
        GUI.Label(new Rect(10, 140, 500, 20), $"({inList}) inList");

    }

    private void Update()
    {
        inList = false;
        foreach (GameObject go in gameController.interactables)
        {
            if (go == gameObject)
            {
                inList = true;
                break;
            }
        }

        if (!inList) gameController.interactables.Add(gameObject);
    }

    private void Reset()
    {
        troubleMaker = GetComponent<Coldrin>();
        interact = GetComponent<Interact>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
}
