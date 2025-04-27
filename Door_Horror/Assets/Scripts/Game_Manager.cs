using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance = null;

    public int roomsPassed = 0; // Includes red & yellow only
    public int redRoomsPassed = 0;
    public int yellowRoomsPassed = 0;
    public int blackRoomsPassed = 0;
    public int blackDoorUses = 0;

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
    
    [Header("Language Settings")]
    public bool langChoosen = false;

    // Keys for room progress
    private const string RedRoomsPassedKey = "RedRoomsPassed";
    private const string YellowRoomsPassedKey = "YellowRoomsPassed";
    private const string BlackRoomsPassedKey = "BlackRoomsPassed";
    private const string BlackDoorUsesKey = "BlackDoorUses";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
            LoadRoomProgress(); // Load saved room progress
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
        SaveRoomProgress(); // Save progress after passing a room

        SceneManager.LoadScene(yellowRoomScene);
    }

    private void HandleRedRoom()
    {
        redRoomsPassed++;
        roomsPassed++;
        justExitedBlackDoor = false;
        SaveRoomProgress(); // Save progress after passing a room

        SceneManager.LoadScene(redRoomScene);
    }

    private void HandleBlackDoor()
    {
        blackDoorUses++;
        justExitedBlackDoor = true;
        SaveRoomProgress(); // Save progress after using a black door

        SceneManager.LoadScene(blackRoomScene);
    }

    public void PassBlackRoom()
    {
        blackRoomsPassed++;
        SaveRoomProgress(); // Save progress after passing a black room
    }

    public void ReturnFromBlackRoom()
    {
        if (blackDoorUses < 3)
        {
            // We do NOT increment roomsPassed here, so red/yellow sequence stays intact
        }

        justExitedBlackDoor = false;
    }

    // New method to save the progress in PlayerPrefs
    private void SaveRoomProgress()
    {
        PlayerPrefs.SetInt(RedRoomsPassedKey, redRoomsPassed);
        PlayerPrefs.SetInt(YellowRoomsPassedKey, yellowRoomsPassed);
        PlayerPrefs.SetInt(BlackRoomsPassedKey, blackRoomsPassed);
        PlayerPrefs.SetInt(BlackDoorUsesKey, blackDoorUses);
        PlayerPrefs.Save();
    }

    // New method to load the saved room progress from PlayerPrefs
    private void LoadRoomProgress()
    {
        redRoomsPassed = PlayerPrefs.GetInt(RedRoomsPassedKey, 0);
        yellowRoomsPassed = PlayerPrefs.GetInt(YellowRoomsPassedKey, 0);
        blackRoomsPassed = PlayerPrefs.GetInt(BlackRoomsPassedKey, 0);
        blackDoorUses = PlayerPrefs.GetInt(BlackDoorUsesKey, 0);
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
        
        roomsPassed = 0;
        redRoomsPassed = 0;
        yellowRoomsPassed = 0;
        blackRoomsPassed = 0;
        blackDoorUses = 0;
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
    
    public void AdjustGameObjectsForLocalization()
    {
        // Get the current localization index based on the selected locale
        int localizationIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        // Determine the tag based on the localization index
        string tag = localizationIndex switch
        {
            0 => "Eng",  // English
            1 => "Rus",  // Russian
            2 => "Esp",  // Spanish
            _ => "Eng",  // Default to English if not found
        };

        // Enable/Disable GameObjects based on localization
        GameObject[] localizedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in localizedObjects)
        {
            obj.SetActive(true);
        }

        // Disable GameObjects for other languages
        string[] allTags = { "Eng", "Rus", "Esp" };
        foreach (var otherTag in allTags)
        {
            if (otherTag != tag)
            {
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag(otherTag);
                foreach (var obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    
    public void AdjustGameObjectsForSoundSettings()
    {
        // Handle Music Objects
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
        foreach (var obj in musicObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.enabled = isMusicOn;
            }
        }

        // Handle Audio Objects
        GameObject[] audioObjects = GameObject.FindGameObjectsWithTag("Audio");
        foreach (var obj in audioObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.enabled = isSoundOn;
            }
        }
    }

    public void AdjustGameObjectsForSubtitlesSettings()
    {
        // Handle Subtitle Objects
        GameObject[] subtitleObjects = GameObject.FindGameObjectsWithTag("Sub");
        foreach (var obj in subtitleObjects)
        {
            Transform childTransform = obj.transform.GetChild(0);
            if (childTransform != null)
            {
                childTransform.gameObject.SetActive(areSubtitlesOn);
            }
        }
    }
}
