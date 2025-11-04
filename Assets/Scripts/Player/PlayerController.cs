using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;

    public float movementSpeed;
    Vector2 moveDir;
    Vector3 lastDir;
    float deadZone = 0.05f;
    [Tooltip("0 = up, 1 = right, 2 = down, 3 = left")]
    public List<GameObject> moveDirObjects;

    [Header("Item Settings")]
    public float pickUpFOV;
    public float pickUpRange;
    public float throwForce;
    public Transform hand;
    public Items currentHeldItem;

    void Update()
    {
        if (!GameController.gameController.active) return; // Freeze the player when paused
        cc.Move(new Vector3(moveDir.x, 0, moveDir.y) * movementSpeed * Time.deltaTime);
    }

    public void SetMoveDir(InputAction.CallbackContext obj)
    {
        moveDir = obj.ReadValue<Vector2>();

        Vector3 dir = new Vector3(moveDir.x, 0, moveDir.y).normalized;

        moveDir = Vector2.ClampMagnitude(moveDir, 1);

        if (dir.sqrMagnitude < deadZone * deadZone)
            return;

        dir.Normalize();
        lastDir = dir;

        RotatePlayer(lastDir);
    }

    public void RotatePlayer(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float snapped = Mathf.Round(angle / 45) * 45;

        Quaternion targetRot = Quaternion.Euler(0, snapped, 0);

        transform.rotation = targetRot;

        bool zDominant = Mathf.Abs(dir.z) >= Mathf.Abs(dir.x);

        ActivateDirObj(0, zDominant && dir.z > 0f);  // Up
        ActivateDirObj(2, zDominant && dir.z < 0f);  // Down
        ActivateDirObj(1, !zDominant && dir.x > 0f); // Right
        ActivateDirObj(3, !zDominant && dir.x < 0f); // Left
    }

    public void ActivateDirObj(int index, bool active)
    {
        if(moveDirObjects[index].activeSelf != active) moveDirObjects[index].SetActive(active);
    }

    public void PickUp(InputAction.CallbackContext obj)
    {
        if (obj.started)
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
            else
            {
                currentHeldItem.transform.SetParent(null);
                currentHeldItem.rb.isKinematic = false;
                currentHeldItem.rb.AddForce(transform.forward * throwForce);
                GameController.gameController.items.Add(currentHeldItem.gameObject);
                currentHeldItem = null;
            }
        }
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            GameObject interactable = GetClosestObject(GameController.gameController.interactables);

            if (interactable != null)
            {
                global::Interact theObject = interactable.GetComponent<Interact>();

                if (theObject.needsEmptyHand)
                {
                    if (currentHeldItem == null)
                        theObject.onInteract.Invoke(this);
                }
                else
                    theObject.onInteract.Invoke(this);
            }
        }
    }

    public void PickUpChosenItem(GameObject pickup)
    {
        if (currentHeldItem == null)
        {
            pickup.transform.parent = hand;
            currentHeldItem = pickup.GetComponent<Items>();
            currentHeldItem.owner = this;
            GameController.gameController.items.Remove(pickup);
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

        float radius = /*Mathf.Tan(pickUpFOV * 0.5f * Mathf.Deg2Rad) */ pickUpRange;
            
        float linDis = Mathf.Sqrt(Mathf.Pow(pickUpRange, 2) + Mathf.Pow(radius, 2));

        Vector3 sideRight = Quaternion.AngleAxis(pickUpFOV * 0.5f, up) * forward;
        Vector3 sideLeft = Quaternion.AngleAxis(-pickUpFOV * 0.5f, up) * forward;


        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + sideRight * linDis);
        Gizmos.DrawLine(transform.position, transform.position + sideLeft * linDis);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * pickUpRange));
    }
}