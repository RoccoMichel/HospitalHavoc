using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text moneyDisplay;
    private GameController gameController;


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        moneyDisplay.text = Mathf.Ceil(gameController.money).ToString();
    }
}
