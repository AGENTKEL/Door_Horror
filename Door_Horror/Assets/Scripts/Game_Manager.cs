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
    
    [Header("Settings Toggles")]
    public bool isMusicOn = true;
    public bool isSoundOn = true;
    public bool areSubtitlesOn = true;
    private const string MusicKey = "Music";
    private const string SoundKey = "Sound";
    private const string SubtitlesKey = "Subtitles";
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
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
    
    public void SetMusic(bool value)
    {
        isMusicOn = value;
        PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
        PlayerPrefs.Save();

        // Find and toggle all Music scripts
        foreach (var music in FindObjectsOfType<Music>(true)) // true includes inactive objects
        {
            music.gameObject.SetActive(value);
        }
    }

    public void SetSound(bool value)
    {
        isSoundOn = value;
        PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
        PlayerPrefs.Save();

        // Find and toggle all Sound scripts
        foreach (var sound in FindObjectsOfType<Audio>(true))
        {
            sound.gameObject.SetActive(value);
        }
    }

    public void SetSubtitles(bool value)
    {
        areSubtitlesOn = value;
        PlayerPrefs.SetInt(SubtitlesKey, value ? 1 : 0);
        PlayerPrefs.Save();

        // Find and toggle all Subtitle scripts
        foreach (var subtitle in FindObjectsOfType<Subtitle>(true))
        {
            subtitle.gameObject.SetActive(value);
        }
    }

    public void LoadSettings()
    {
        isMusicOn = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        isSoundOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
        areSubtitlesOn = PlayerPrefs.GetInt(SubtitlesKey, 1) == 1;
    }
}
