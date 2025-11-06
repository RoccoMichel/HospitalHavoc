using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    [SerializeField] GameObject target;
    internal PlayerController player;
    private RectTransform rectTransform;
    private Image sprite;
    private Canvas canvas;

    private void Start()
    {
        sprite = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        try 
        {
            target = player.GetClosestObject(GameController.gameController.items);
            if (target.IsUnityNull()) {
                transform.rotation = Quaternion.Euler(0, 0, 45);
                target = player.GetClosestObject(GameController.gameController.interactables);
            }
            else transform.rotation = Quaternion.Euler(0, 0, 0);

            Vector3 worldPos = target.transform.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                null,
                out uiPos
            );

            rectTransform.anchoredPosition = uiPos;
            sprite.enabled = true;
        }
        catch 
        {
            sprite.enabled = false;
            return; 
        }
    }
}
