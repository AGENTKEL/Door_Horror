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

    private RewardedController rewardedController;
    private bool waitingForRewardedAdToComplete = false;
    private System.Action onRewardedCompleteCallback;

    private void Start()
    {
        rewardedController = FindObjectOfType<RewardedController>();
    }

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
        RewardedController rewardedController = FindObjectOfType<RewardedController>();
        if (rewardedController != null)
        {
            rewardedController.ShowTheAd(RestartScene);
        }
    }

    public void SkipForAd()
    {
        RewardedController rewardedController = FindObjectOfType<RewardedController>();
        if (rewardedController != null)
        {
            rewardedController.ShowTheAd(ProceedRoomSkipping);
        }
    }

    private void ProceedRoomSkipping()
    {
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name == "Black")
        {
            Game_Manager.instance.PassBlackRoom();
            if (Game_Manager.instance.blackRoomsPassed >= 3)
            {
                SceneManager.LoadScene("Black_End");
                return;
            }
            Game_Manager.instance.OnDoorEntered(FindObjectOfType<Door>().doorColor);
            return;
        }

        if (Game_Manager.instance.roomsPassed >= 9)
        {
            SceneManager.LoadScene("End");
            return;
        }

        Game_Manager.instance.OnDoorEntered(FindObjectOfType<Door>().doorColor);
    }

    private void OnRewardedAdComplete()
    {
        waitingForRewardedAdToComplete = false;
        onRewardedCompleteCallback?.Invoke();
        onRewardedCompleteCallback = null;
    }

    // Pause
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
    
    public void TriggerBlackScreenAndSwitchScene()
    {
        StartCoroutine(FadeInBlackScreenSwitchScene());
    }

    private IEnumerator FadeInBlackScreenSwitchScene()
    {
        float fadeDuration = 3f; // Duration of the fade (in seconds)
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

        PlayerDeath();
    }
}
