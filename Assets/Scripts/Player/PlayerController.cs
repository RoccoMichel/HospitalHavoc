using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    public float movementSpeed;
    Vector2 moveDir;

    void Update()
    {
        cc.Move(new Vector3(moveDir.x, 0, moveDir.y) * movementSpeed * Time.deltaTime);
    }

    public void SetMoveDir(InputAction.CallbackContext obj)
    {
        moveDir = obj.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext obj)
    {

    }

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
}