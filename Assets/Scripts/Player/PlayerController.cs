using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    public float movementSpeed;
    Vector2 moveDir;

    [Header("Item Settings")]
    public float pickUpFOV;
    public float pickUpRange;
    public Transform hand;
    public GameObject currentHeldItem;

    void Update()
    {
        cc.Move(new Vector3(moveDir.x, 0, moveDir.y) * movementSpeed * Time.deltaTime);
    }

    public void SetMoveDir(InputAction.CallbackContext obj)
    {
        moveDir = obj.ReadValue<Vector2>();
    }

    public void PickUp(InputAction.CallbackContext obj)
    {
        if (currentHeldItem == null)
        {
            GameObject itemToPickUp = GetClosestObject();

            if (itemToPickUp != null)
            {
                itemToPickUp.transform.parent = hand;
                currentHeldItem = itemToPickUp;

                GameController.gameController.items.Remove(itemToPickUp);
            }
        }
    }

    public void Interact(InputAction.CallbackContext obj)
    {

    }

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public GameObject GetClosestObject()
    {
        GameObject closest = null;
        float closestDist = pickUpRange;

        foreach (GameObject item in GameController.gameController.items)
        {
            if (Vector3.Dot(transform.forward, (item.transform.position - transform.position).normalized) >= Mathf.Cos(0.5f * pickUpFOV * Mathf.Deg2Rad))
            {
                if (Vector3.Distance(transform.position, item.transform.position) < closestDist)
                {
                    closest = item;
                    closestDist = Vector3.Distance(transform.position, item.transform.position);
                }
            }
        }

        return closest;
    }

    void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 up = transform.up;

        float radius = Mathf.Tan(pickUpFOV * 0.5f * Mathf.Deg2Rad) * pickUpRange;

        float linDis = Mathf.Sqrt(Mathf.Pow(pickUpRange, 2) + Mathf.Pow(radius, 2));

        Vector3 sideRight = Quaternion.AngleAxis(pickUpFOV * 0.5f, up) * forward;
        Vector3 sideLeft = Quaternion.AngleAxis(-pickUpFOV * 0.5f, up) * forward;


        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + sideRight * linDis);
        Gizmos.DrawLine(transform.position, transform.position + sideLeft * linDis);
    }
}