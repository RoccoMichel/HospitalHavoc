using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    public float movementSpeed;
    public Transform orentation;
    Vector2 moveDir;
    float yRot = 0;
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

        // up, down, left or right
        if (moveDir.x > 0 && Mathf.Abs(moveDir.y) < 0.25f)  //right
        {
            yRot = 0;
            ActivateDirObj(1);
            orentation.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (moveDir.x < 0 && Mathf.Abs(moveDir.y) < 0.25f) //left
        {
            yRot = 0;
            ActivateDirObj(3);
            orentation.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (Mathf.Abs(moveDir.x) < 0.25f && moveDir.y > 0) //up
        {
            yRot = 0;
            ActivateDirObj(0);
            orentation.rotation = Quaternion.identity;
        }
        else if (Mathf.Abs(moveDir.x) < 0.25f && moveDir.y < 0) //down
        {
            yRot = 0;
            ActivateDirObj(2);
            orentation.rotation = Quaternion.Euler(0, 180, 0);
        }

        //diaginol
        else if (moveDir.x > 0 && moveDir.y > 0)    //up right
        {
            yRot = 45;
            ActivateDirObj(moveDir.x > moveDir.y ? 1 : 0);
            orentation.rotation = Quaternion.Euler(0, 45, 0);
        }
        else if (moveDir.x > 0 && moveDir.y < 0)    //down right
        {
            yRot = -45;
            ActivateDirObj(moveDir.x > -moveDir.y ? 1 : 2);
            orentation.rotation = Quaternion.Euler(0, 135, 0);
        }
        else if (moveDir.x < 0 && moveDir.y > 0)    //up left
        {
            yRot = -45;
            ActivateDirObj(-moveDir.x > moveDir.y ? 3 : 0);
            orentation.rotation = Quaternion.Euler(0, 315, 0);
        }
        else if (moveDir.x < 0 && moveDir.y < 0)    //down left
        {
            yRot = 45;
            ActivateDirObj(moveDir.x > -moveDir.y ? 3 : 2);
            orentation.rotation = Quaternion.Euler(0, 225, 0);
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
            Debug.Log("Interacted");

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
            if (Vector3.Dot(orentation.forward, (item.transform.position - transform.position).normalized) >= Mathf.Cos(0.5f * pickUpFOV * Mathf.Deg2Rad))
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
        Vector3 forward = orentation.forward;
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