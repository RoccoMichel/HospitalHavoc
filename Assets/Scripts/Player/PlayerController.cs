using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    public float movementSpeed;
    Vector2 moveDir;

    [Tooltip("0 = up, 1 = right, 2 = down, 3 = left")]
    public List<GameObject> moveDirObjects;

    [Header("Item Settings")]
    public float pickUpFOV;
    public float pickUpRange;
    public Transform hand;
    public Items currentHeldItem;

    void Update()
    {
        cc.Move(new Vector3(moveDir.x, 0, moveDir.y) * movementSpeed * Time.deltaTime);
    }

    public void SetMoveDir(InputAction.CallbackContext obj)
    {
        moveDir = obj.ReadValue<Vector2>();

        float yRot = 0;

        // up, down, left or right
        if (moveDir.x > 0 && Mathf.Abs(moveDir.y) < 0.25f)
            ActivateDirObj(1);
        else if (moveDir.x < 0 && Mathf.Abs(moveDir.y) < 0.25f)
            ActivateDirObj(3);
        else if (Mathf.Abs(moveDir.x) < 0.25f && moveDir.y > 0)
            ActivateDirObj(0);
        else if (Mathf.Abs(moveDir.x) < 0.25f && moveDir.y < 0)
            ActivateDirObj(2);

        //diaginol
        else if (moveDir.x > 0 && moveDir.y > 0)
        {
            yRot = 45;
            ActivateDirObj(moveDir.x > moveDir.y ? 1 : 0);
        }
        else if (moveDir.x > 0 && moveDir.y < 0)
        {
            yRot = -45;
            ActivateDirObj(moveDir.x > -moveDir.y ? 1 : 2);
        }
        else if (moveDir.x < 0 && moveDir.y > 0)
        {
            yRot = -45;
            ActivateDirObj(-moveDir.x > moveDir.y ? 3 : 0);
        }
        else if (moveDir.x < 0 && moveDir.y < 0)
        {
            yRot = 45;
            ActivateDirObj(moveDir.x > -moveDir.y ? 3 : 2);
        }

        transform.rotation = Quaternion.Euler(0, yRot, 0);

        moveDir = Vector2.ClampMagnitude(moveDir, 1);
    }

    public void ActivateDirObj(int index)
    {
        foreach (GameObject obj in moveDirObjects)
        {
            if(obj != moveDirObjects[index])
                obj.SetActive(false);
        }

        moveDirObjects[index].SetActive(true);
    }

    public void PickUp(InputAction.CallbackContext obj)
    {
        if (currentHeldItem == null)
        {
            GameObject itemToPickUp = GetClosestObject(GameController.gameController.items);

            if (itemToPickUp != null)
            {
                itemToPickUp.transform.parent = hand;
                currentHeldItem = itemToPickUp.GetComponent<Items>();

                GameController.gameController.items.Remove(itemToPickUp);
            }
        }
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        GameObject interactable = GetClosestObject(GameController.gameController.interactables);

        if (interactable != null)
        {
            interactable.GetComponent<Interact>().onInteract.Invoke(this);
        }
    }

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public GameObject GetClosestObject(List<GameObject> search)
    {
        GameObject closest = null;
        float closestDist = pickUpRange;

        foreach (GameObject item in search)
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