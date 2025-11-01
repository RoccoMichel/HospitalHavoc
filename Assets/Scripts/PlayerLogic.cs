using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    private InputAction interactAction;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (interactAction.WasPressedThisFrame()) Interact();
    }

    public virtual void Interact()
    {
        Debug.Log(gameObject.name + " interacted");
    }
}
