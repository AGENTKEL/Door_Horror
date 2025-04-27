using System.Collections;
using System.Collections.Generic;
using CharacterScript;
using UnityEngine;

public class ClownManager : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    public float minLookTime = 3f;
    public float maxLookTime = 5f;

    public float rotationSpeed = 90f; // degrees per second
    public AudioSource musicSource;

    public Collider triggerStartCollider; // first trigger collider (on this GameObject)
    public Collider secondCollider;       // second collider (on this GameObject)

    private bool isLookingAtPlayers = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool rotating = false;
    private bool routineStarted = false;
    
    public Clock clock;

    private void Start()
    {
        originalRotation = transform.rotation;
        secondCollider.enabled = false; // Disable second collider at start
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                rotating = false;

                if (isLookingAtPlayers)
                {
                    musicSource.Stop();
                    secondCollider.enabled = true; // Enable second collider when clown fully rotated
                }
                else
                {

                    secondCollider.enabled = false; // Disable second collider when clown turns back
                }
            }
        }
    }

    private IEnumerator ControlRoutine()
    {
        while (true)
        {
            // 1. Rotate back to original (clown looking away)
            isLookingAtPlayers = false;
            RotateTo(originalRotation);
            secondCollider.enabled = false;
            musicSource.Play(); // 🎵 Music resumes immediately when clown looks away
            yield return new WaitUntil(() => !rotating);

            // 2. Wait a random time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 3. Rotate to look at players
            isLookingAtPlayers = true;
            RotateTo(Quaternion.Euler(transform.eulerAngles + new Vector3(0, 180, 0)));
            musicSource.Stop(); // 🎵 Music stops immediately when clown starts looking at players
            yield return new WaitUntil(() => !rotating);

            // 4. Look at players for random time
            float lookTime = Random.Range(minLookTime, maxLookTime);
            yield return new WaitForSeconds(lookTime);
        }
    }

    private void RotateTo(Quaternion target)
    {
        targetRotation = target;
        rotating = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !routineStarted)
        {
            StartCoroutine(ControlRoutine());
            routineStarted = true;
            triggerStartCollider.enabled = false;
            if (clock != null)
            {
                clock.clockRunning = true; // <-- START THE CLOCK
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (secondCollider.enabled && other.CompareTag("Player"))
        {
            FPSController playerController = other.GetComponent<FPSController>();

            if (playerController != null)
            {
                if (IsPlayerMoving(playerController))
                {
                    PlayerUI playerUI = other.GetComponent<PlayerUI>();
                    if (playerUI != null)
                    {
                        playerUI.PlayerDeath();
                    }
                }
            }
        }
    }

// Helper method to check movement
    private bool IsPlayerMoving(FPSController player)
    {
        float inputHorizontal = player.joystick != null && player.joystick.Horizontal != 0
            ? player.joystick.Horizontal
            : Input.GetAxis("Horizontal");
        float inputVertical = player.joystick != null && player.joystick.Vertical != 0
            ? player.joystick.Vertical
            : Input.GetAxis("Vertical");

        return Mathf.Abs(inputHorizontal) > 0.1f || Mathf.Abs(inputVertical) > 0.1f;
    }
    
    public void KillPlayer()
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerUI playerUI = player.GetComponent<PlayerUI>();
            if (playerUI != null)
            {
                playerUI.PlayerDeath();
            }
        }
    }
}
