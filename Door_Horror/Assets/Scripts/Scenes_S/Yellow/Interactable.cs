using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Key, Note, NoteCar, Toy, Gift }

public class Interactable : MonoBehaviour
{
    public ItemType itemType;
    public Sprite itemSprite;

    private bool isMovingToTarget = false;
    private bool isHeldByPlayer = false;
    private Transform target;

    public float moveSpeed = 5f;

    void Update()
    {
        if (isMovingToTarget && target != null)
        {
            // Smoothly move and rotate toward target
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * moveSpeed);

            // Snap into place when close enough
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                transform.SetParent(target);                      // Become child
                transform.localPosition = Vector3.zero;           // Reset local position
                transform.localRotation = Quaternion.identity;    // Reset local rotation

                isMovingToTarget = false;
                isHeldByPlayer = true;
            }
        }
    }

    public void MoveTo(Transform newTarget)
    {
        if (!isHeldByPlayer)
        {
            target = newTarget;
            isMovingToTarget = true;
        }
        else
        {
            Destroy(gameObject); // Second interaction destroys it
        }
    }
    
    public void Interact(Inventory inventory)
    {
        if (itemType == ItemType.Toy)
        {
            Toy toy = GetComponent<Toy>();
            toy.Interact(inventory);
            return;
        }

        // Default interaction logic (e.g., pickup or note reading)
        if (itemType == ItemType.Note)
        {
            MoveTo(inventory.playerCameraInteract.noteTarget); // Assuming inventory or context provides this
        }
        
        else if (itemType == ItemType.NoteCar)
        {
            MoveTo(inventory.playerCameraInteract.noteTarget); // Assuming inventory or context provides this
            FindObjectOfType<CarManager>().UnlockDoor();
        }
        else
        {
            inventory.AddItem(this);
        }
    }
}
