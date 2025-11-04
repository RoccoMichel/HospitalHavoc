using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public int activePage = 0;
    [SerializeField] private Sprite[] pages;
    [SerializeField] private Image pageDisplay;


    private void Start()
    {
        DisplayPage(activePage);
    }

    public void NextPage()
    {
        activePage = Mathf.Clamp(activePage + 1, 0, pages.Length - 1);
        DisplayPage(activePage);
    }

    public void PreviousPage()
    {
        activePage = Mathf.Clamp(activePage - 1, 0, pages.Length - 1);
        DisplayPage(activePage);
    }

    private void DisplayPage(int index)
    {
        if (index > pages.Length) { Debug.LogWarning("Target Recipe Book index exceeds array Length!"); return; }
        pageDisplay.sprite = pages[index];
    }
}
