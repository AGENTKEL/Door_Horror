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
                    FindObjectOfType<InterstitialController>().OnRoomPassed(() => {
                        // Code to load next level here
                        SceneManager.LoadScene("Black_End");
                    });
                    SceneManager.LoadScene("Black_End");
                    return;
                }
                
                Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
            }
            
            if (Game_Manager.instance.roomsPassed >= 9)
            {
                FindObjectOfType<InterstitialController>().OnRoomPassed(() => {
                    // Code to load next level here
                    SceneManager.LoadScene("End");
                });
                SceneManager.LoadScene("End");
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Yellow")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.yellowRoomsPassed++;

                Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Red")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.redRoomsPassed++;

                Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
                return;
            }
            
            
            
            Game_Manager.instance.OnDoorEntered(linkedDoor.doorColor);
        }
    }
}
