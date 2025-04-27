using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public Door linkedDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && linkedDoor.open)
        {
            if (SceneManager.GetActiveScene().name == "Black")
            {
                Game_Manager.instance.PassBlackRoom();
                if (Game_Manager.instance.blackRoomsPassed >= 3)
                {
                    SceneManager.LoadScene("Black_End");
                    return;
                }
                
                Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
                return;
            }
            
            if (Game_Manager.instance.roomsPassed >= 9)
            {
                SceneManager.LoadScene("End");
                return;
            }
            
            Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
        }
    }
}
