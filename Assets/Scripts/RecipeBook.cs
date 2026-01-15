using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public List<Transform> seleksen;
    public RectTransform seleksenBackrond;
    public static int activePage = 0;
    [SerializeField] private Image pageDisplay;
    public AvailableRecipes availableRecipes;
    bool fired;
    InputAction navigate;
    InputAction cancel;


    private void Start()
    {
        GameController.gameController.active = false;
        navigate = InputSystem.actions.FindAction("Navigate");
        cancel = InputSystem.actions.FindAction("Cancel");
        DisplayPage(activePage);

        for (int i = 1; i < availableRecipes.pages.Length; i++) {
            seleksen.Add(Instantiate(seleksen[0]));
            seleksen.Last().SetParent(seleksen[0].parent);
            seleksen.Last().localScale = Vector3.one;
            seleksen.Last().GetComponentInChildren<TextMeshProUGUI>().text = $"{i+1}";
        }

        seleksenBackrond.sizeDelta = new Vector2(availableRecipes.pages.Length * 75, 60);
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
        activePage = Mathf.Clamp(activePage + 1, 0, availableRecipes.pages.Length - 1);
        DisplayPage(activePage);
        fired = true;
    }

    public void PreviousPage()
    {
        if (fired) return;
        activePage = Mathf.Clamp(activePage - 1, 0, availableRecipes.pages.Length - 1);
        DisplayPage(activePage);
        fired = true;
    }

    public void SetPage(Transform aktivButen)
    {
        int i = aktivButen.GetSiblingIndex();
        if (fired) return;
        activePage = Mathf.Clamp(i, 0, availableRecipes.pages.Length - 1);
        DisplayPage(activePage);
        fired = true;
    }

    private void DisplayPage(int index)
    {
        if (index > availableRecipes.pages.Length) { Debug.LogWarning("Target Recipe Book index exceeds array Length!"); return; }
        pageDisplay.sprite = availableRecipes.pages[index];
    }
}
