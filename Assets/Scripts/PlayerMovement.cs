using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;

    [Header("Walking")]
    public float speed = 5;
    [SerializeField] private float accelerationSpeed = 4;
    [SerializeField] private float decelerationSpeed = 2;

    [Header("Gravity")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravityStrength = 1;
    private const float GRAVITY = 9.81f;
    private bool isGrounded;
    private Vector3 velocity;

    private CharacterController controller;
    private InputAction moveAction;
    private Vector2 moveValue;
    private Vector2 direction;
    private float momentum;


    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        controller = GetComponent<CharacterController>();
        if (groundCheck == null) { Debug.LogError(gameObject.name + " has no ground check assigned!"); Debug.Break(); }
    }

    private void Update()
    {
        if (!canMove) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.08f, groundLayer);
        velocity.y = isGrounded ? 0 : velocity.y - GRAVITY * gravityStrength * Time.deltaTime;

        moveValue = moveAction.ReadValue<Vector2>();
        momentum = Mathf.Clamp01(moveValue == Vector2.zero ? momentum - decelerationSpeed : momentum + accelerationSpeed);
        direction = Vector2.Lerp(direction, moveValue, 0.015f);

        Vector3 move = momentum * speed * (transform.right * direction.x + transform.forward * direction.y);
        move += velocity;

        controller.Move(Time.deltaTime * move);
    }

    public void SetSpeed(float f) => speed = f;
}