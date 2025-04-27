using System;
using System.Collections;
using System.Collections.Generic;
using CharacterScript;
using DoorScript;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private FPSController playerController;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Image blackImage;
    private bool isPaused;
    

    public void PlayerDeath()
    {
        playerController.isDead = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        deadScreen.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void PlayAd()
    {
        RestartScene();
    }
    
    public void SkipForAd()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "Black")
        {
            Game_Manager.instance.PassBlackRoom();
            Game_Manager.instance.OnDoorEntered(FindObjectOfType<Door>().doorColor);
            return;
        }
        Game_Manager.instance.OnDoorEntered(FindObjectOfType<Door>().doorColor);
    }
    
    //Pause
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            isPaused = false;
        }
        else
        {
            pauseScreen.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    
    public void TriggerBlackScreenAndDie()
    {
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator FadeInBlackScreen()
    {
        float fadeDuration = 1.5f; // Duration of the fade (in seconds)
        float elapsedTime = 0f;

        Color color = blackImage.color;
        color.a = 0f;
        blackImage.color = color;
        blackImage.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackImage.color = color;
            yield return null;
        }

        // After fade is complete, call PlayerDeath
        PlayerDeath();
    }
}
