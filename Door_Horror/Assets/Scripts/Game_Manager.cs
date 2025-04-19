using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
public static Game_Manager instance = null;

    [SerializeField] private int roomsPassed = 0; // Includes red & yellow only
    [SerializeField] private int redRoomsPassed = 0;
    [SerializeField] private int yellowRoomsPassed = 0;
    [SerializeField] private int blackDoorUses = 0;

    public int blackRoomsPassed = 0;

    private bool justExitedBlackDoor = false;

    [Header("Scene Names")]
    public string yellowRoomScene = "Yellow";
    public string redRoomScene = "Red";
    public string blackRoomScene = "Black";

    [Header("Passed Level Indexes")]
    public List<int> yellowRoomsUsed = new List<int>();
    public List<int> redRoomsUsed = new List<int>();
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetAllRoomTracking();
    }

    public void OnDoorEntered(DoorScript.DoorColor doorColor)
    {
        switch (doorColor)
        {
            case DoorScript.DoorColor.Black:
                HandleBlackDoor();
                break;
            case DoorScript.DoorColor.Red:
                HandleRedRoom();
                break;
            case DoorScript.DoorColor.Yellow:
                HandleYellowRoom();
                break;
        }
    }

    private void HandleYellowRoom()
    {
        yellowRoomsPassed++;
        roomsPassed++;
        justExitedBlackDoor = false;

        SceneManager.LoadScene(yellowRoomScene);
    }

    private void HandleRedRoom()
    {
        redRoomsPassed++;
        roomsPassed++;
        justExitedBlackDoor = false;

        SceneManager.LoadScene(redRoomScene);
    }

    private void HandleBlackDoor()
    {
        blackDoorUses++;
        blackRoomsPassed++; // ✅ Count it as passed
        justExitedBlackDoor = true;

        SceneManager.LoadScene(blackRoomScene);
    }

    // Called manually from black room exit
    public void ReturnFromBlackRoom()
    {
        if (blackDoorUses < 3)
        {
            // We do NOT increment roomsPassed here, so red/yellow sequence stays intact
        }

        justExitedBlackDoor = false;
    }

    public DoorScript.DoorColor GetNextExpectedRoomColor()
    {
        if (justExitedBlackDoor)
            return DoorScript.DoorColor.Yellow;

        int cyclePosition = roomsPassed % 3;
        return cyclePosition == 2 ? DoorScript.DoorColor.Red : DoorScript.DoorColor.Yellow;
    }

    public DoorScript.DoorColor GetNextColorAfterBlackRoom()
    {
        int cyclePosition = roomsPassed % 3;
        return cyclePosition == 2 ? DoorScript.DoorColor.Red : DoorScript.DoorColor.Yellow;
    }

    public bool ShouldSpawnBlackDoor()
    {
        return blackDoorUses < 3 && (roomsPassed == 0 || roomsPassed == 3 || roomsPassed == 6);
    }

    public void ResetAllRoomTracking()
    {
        yellowRoomsUsed.Clear();
        redRoomsUsed.Clear();
    }
}
