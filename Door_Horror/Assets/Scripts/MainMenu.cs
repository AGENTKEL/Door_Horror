using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject langObject;
    
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle subtitlesToggle;

    private void Awake()
    {
        Game_Manager.instance.LoadSettings();
    }

    private void Start()
    {
        // Temporarily remove listeners to avoid triggering toggle methods
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        subtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);

        // Set toggle values without triggering events
        musicToggle.isOn = Game_Manager.instance.isMusicOn;
        soundToggle.isOn = Game_Manager.instance.isSoundOn;
        subtitlesToggle.isOn = Game_Manager.instance.areSubtitlesOn;

        // Re-attach listeners
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        subtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggleChanged);
    }
    
    private void OnMusicToggleChanged(bool value)
    {
        Game_Manager.instance.SetMusic(value);
    }

    private void OnSoundToggleChanged(bool value)
    {
        Game_Manager.instance.SetSound(value);
    }

    private void OnSubtitlesToggleChanged(bool value)
    {
        Game_Manager.instance.SetSubtitles(value);
    }
    
    
    public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void ChoseLang()
    {
        menuObject.SetActive(true);
        langObject.SetActive(false);
    }
    
    public void ToggleMusic()
    {
        bool newValue = !Game_Manager.instance.isMusicOn;
        Game_Manager.instance.SetMusic(newValue);
    }

    public void ToggleSound()
    {
        bool newValue = !Game_Manager.instance.isSoundOn;
        Game_Manager.instance.SetSound(newValue);
    }

    public void ToggleSubtitles()
    {
        bool newValue = !Game_Manager.instance.areSubtitlesOn;
        Game_Manager.instance.SetSubtitles(newValue);
    }
}
