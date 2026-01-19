using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    AnimationController ac;
    public Animator tempAnimasn;
    public int playerInt;
    public Transform DireksenPonter;
    [Header("Movement Settings")]
    public float movementSpeed;
    public bool canMove = true;
    public LayerMask movingPlatformMask;
    public float maxPlatformDist;
    Vector2 moveDir;
    Vector3 lastDir;
    float deadZone = 0.05f;
    [Tooltip("0 = up, 1 = right, 2 = down, 3 = left")]
    public List<GameObject> moveDirObjects, player2MoveDirObjects;

    public ParticleSystem wakingpartiols;

    [Header("Dash Settings")]
    public float dashForce = 15;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1;
    public bool canDash = true;
    public ParticleSystem dashEfect;
    public Image DashColdown;
    float timeTillDash = 0;

    [Header("Item Settings")]
    public float pickUpFOV;
    public float pickUpRange;
    public float throwForce;
    public Transform hand;
    public Items currentHeldItem;

    MovingPlatform mp;
    Vector3 platformMove;
    void Update()
    {
        //SnapY();
        if (!GameController.gameController.active) return; // Freeze the player when paused

        if (!canDash)
        {
            DashColdown.gameObject.SetActive(true);
            timeTillDash += Time.deltaTime;
            DashColdown.fillAmount = timeTillDash / dashCooldown;
        }
        else
            DashColdown.gameObject.SetActive(false);

        Vector3 move = new Vector3(moveDir.x, 0, moveDir.y);
        tempAnimasn.SetBool("Run", canMove && move.magnitude > 0);

        if (!cc.isGrounded)
            move.y = -2;



        if (canMove)
            cc.Move(move * (movementSpeed * Time.deltaTime));
        else
            cc.Move(lastDir * (dashForce * Time.deltaTime));

        CheckForPlatform();

        if (mp != null)
            cc.Move(platformMove);

        GameObject clossestItem = GetClosestObject(GameController.gameController.interactables);
        if (GameController.gameController.items.Count > 0)
            clossestItem = GetClosestObject(GameController.gameController.items);

        GameObject clossestInteractable = GetClosestObject(GameController.gameController.interactables, false);
        GameObject clossest = null;
        if(clossestItem && clossestInteractable)
            clossest = Vector3.Distance(transform.position, clossestItem.transform.position) <
                       Vector3.Distance(transform.position, clossestInteractable.transform.position)
                           ? clossestItem
                           : clossestInteractable;

        if (clossestInteractable) {
            Interact interactable = clossestInteractable.GetComponent<Interact>();

            if (interactable != null) {
                if (interactable.needsEmptyHand) {
                    if (currentHeldItem == null) 
                        interactable.canInteract = true;
                    else interactable.canInteract = false;
                }

                if (interactable.needsItem) {
                    if (currentHeldItem != null) 
                        interactable.canInteract = true;
                    else interactable.canInteract = false;
                }
            }
        }
        if (moveDir.magnitude == 0) DireksenPonter.gameObject.SetActive(false);
        else  {
            DireksenPonter.gameObject.SetActive(true);
            DireksenPonter.forward = -transform.forward; 
        }
    }

    void CheckForPlatform()
    {
        platformMove = Vector3.zero;
        mp = null;

        if (cc.isGrounded)
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, maxPlatformDist, movingPlatformMask))
            {
                MovingPlatform newMP = hit.collider.GetComponent<MovingPlatform>();

                if (newMP != null)
                {
                    mp = newMP;
                    platformMove = mp.GetPlatformMovement();
                }
            }
        }
    }

    public void SnapY()
    {
        if (cc != null)
        {
            // Keep current X and Z, set Y to snapY
            Vector3 pos = transform.position;
            pos.y = 1;
            cc.enabled = false;   // Disable temporarily to avoid fighting physics
            transform.position = pos;
            cc.enabled = true;
        }
        else
        {
            // Works even if you�re not using a CharacterController
            Vector3 pos = transform.position;
            pos.y = 1;
            transform.position = pos;
        }
    }

    public void SetMoveDir(InputAction.CallbackContext obj)
    {
        if (canMove)
        {
            moveDir = obj.ReadValue<Vector2>();

            Vector3 dir = new Vector3(moveDir.x, 0, moveDir.y).normalized;

            moveDir = Vector2.ClampMagnitude(moveDir, 1);

            if (dir.sqrMagnitude < deadZone * deadZone)
            {
                ac.StopAni(GetCurrentActiveDirObj());
                return;
            }

            dir.Normalize();
            lastDir = dir;

            RotatePlayer(lastDir);
            ac.PlayAni(GetCurrentActiveDirObj());
        }
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

    public GameObject GetCurrentActiveDirObj()
    {
        foreach (GameObject obj in moveDirObjects)
        {
            if (obj.activeSelf)
                return obj;
        }

        return null;
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            GameObject itemToPickUp = GetClosestObject(GameController.gameController.items);
            GameObject interactable = GetClosestObject(GameController.gameController.interactables);

            bool pickedUpAnItem = false;

            //Pick Up Item
            if (currentHeldItem == null)
            {
                if (itemToPickUp != null)
                {
                    itemToPickUp.transform.parent = hand;
                    currentHeldItem = itemToPickUp.GetComponent<Items>();

                    currentHeldItem.owner = this;

                    GameController.gameController.items.Remove(itemToPickUp);

                    pickedUpAnItem = true;
                }
            }

            if (!pickedUpAnItem)
            {
                if (interactable != null)
                {
                    //Interact
                    Interact theObject = interactable.GetComponent<Interact>();

                    if (theObject.needsEmptyHand)
                    {
                        if (currentHeldItem == null)
                        {
                            theObject.onInteract.Invoke(this);
                        }
                    }
                    else if (theObject.needsItem)
                    {
                        if (currentHeldItem != null)
                            theObject.onInteract.Invoke(this);
                    }
                    else
                        theObject.onInteract.Invoke(this);
                }
                else
                {
                    //Throw Item
                    if (currentHeldItem != null)
                    {
                        currentHeldItem.transform.SetParent(null);
                        currentHeldItem.rb.isKinematic = false;
                        currentHeldItem.rb.AddForce(transform.forward * throwForce);
                        GameController.gameController.items.Add(currentHeldItem.gameObject);
                        currentHeldItem = null;
                    }
                }
            }
        }
    }

    public void PickUpChosenItem(GameObject pickup)
    {
        if (currentHeldItem == null && pickup != null)
        {
            pickup.transform.parent = hand;
            currentHeldItem = pickup.GetComponent<Items>();
            currentHeldItem.owner = this;
            GameController.gameController.items.Remove(pickup);
        }
    }

    public void StartDash(InputAction.CallbackContext obj)
    {
        if (obj.started && canDash)
        {
            timeTillDash = 0;
            dashEfect.Play();
            canMove = false;
            canDash = false;

            ac.StopAni(GetCurrentActiveDirObj());

            StartCoroutine(StopDashing());
        }
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashDuration);

        canMove = true;
        //moveDir = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown);
        
        canDash = true;
        DashColdown.fillAmount = 0;
    }

    public void StopMoveing(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            moveDir = Vector2.zero;
        }
    }

    void Awake()
    {
        GameController.gameController.canvasManager.RequestPlayerHighlight(this);

        cc = GetComponent<CharacterController>();
        ac = GetComponent<AnimationController>();
    }

    public void SetPlayerModel()
    {
        if (playerInt == 2)
        {
            moveDirObjects[0].SetActive(false);
            player2MoveDirObjects[0].SetActive(true);

            moveDirObjects = player2MoveDirObjects;
        }
    }

    public GameObject GetClosestObject(List<GameObject> search, bool needCanInteract = true)
    {
        GameObject closest = null;
        float closestDist = pickUpRange;

        foreach (GameObject item in search)
        {
            if (Vector3.Dot(transform.forward, (item.transform.position - transform.position).normalized) >= Mathf.Cos(0.5f * pickUpFOV * Mathf.Deg2Rad))
            {
                if (Vector3.Distance(transform.position, item.transform.position) < closestDist)
                {
                    if (needCanInteract && item.GetComponent<Interact>() != null)
                        if(!item.GetComponent<Interact>().canInteract || !item.GetComponent<Interact>().overrideInteract)
                            continue;

                    closest = item;
                    closestDist = Vector3.Distance(transform.position, item.transform.position);
                }
            }
        }

        return closest;
    }

    public void ControllerDisconected(PlayerInput obj)
    {
        print("Player: <color=red>" + (obj.playerIndex + 1) + "</color> Has Disconnected");

        OnPlayerJoin.instance.OnPlayerLeave(obj);

        Destroy(gameObject);
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