using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorBlack : MonoBehaviour
{
    [SerializeField] private Door doorComponent;

    private IEnumerator ActivateComponent()
    {
        yield return new WaitForSeconds(0.5f);

        doorComponent.enabled = true;
    }

    public void UnlockBlackDoor()
    {
        doorComponent.isLocked = false;
    }
}
