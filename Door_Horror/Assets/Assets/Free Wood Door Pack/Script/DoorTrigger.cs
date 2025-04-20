using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door linkedDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && linkedDoor.open)
        {
            Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
        }
    }
}
