using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public int activePage = 0;
    [SerializeField] private Sprite[] pages;
    [SerializeField] private Image pageDisplay;
    private bool fired;
    private InputAction navigate;
    private InputAction cancel;


    private void Start()
    {
        GameController.gameController.active = false;
        navigate = InputSystem.actions.FindAction("Navigate");
        cancel = InputSystem.actions.FindAction("Cancel");
        DisplayPage(activePage);
    }

    private void Update()
    {
        if (cancel.WasPressedThisFrame()) CloseBook();

        if (navigate.ReadValue<Vector2>().x < 0.1f && navigate.ReadValue<Vector2>().x > -0.1f) fired = false;
        else if (navigate.ReadValue<Vector2>().x > 0.8f) NextPage();
        else if (navigate.ReadValue<Vector2>().x < -0.8f) PreviousPage();
    }

    public void CloseBook()
    {
        GameController.gameController.active = true;
        Destroy(gameObject);
    }

    public void NextPage()
    {
        if (fired) return;
        activePage = Mathf.Clamp(activePage + 1, 0, pages.Length - 1);
        DisplayPage(activePage);
        fired = true;
    }

    public void PreviousPage()
    {
        if (fired) return;
        activePage = Mathf.Clamp(activePage - 1, 0, pages.Length - 1);
        DisplayPage(activePage);
        fired = true;
    }

    private void DisplayPage(int index)
    {
        if (index > pages.Length) { Debug.LogWarning("Target Recipe Book index exceeds array Length!"); return; }
        pageDisplay.sprite = pages[index];
    }
}
