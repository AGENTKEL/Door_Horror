using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownManager : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    public float minLookTime = 3f;
    public float maxLookTime = 5f;

    public AudioSource musicSource;

    private bool isLookingAtPlayers = false;

    private Quaternion originalRotation;
    private Quaternion lookingRotation;

    private void Start()
    {
        originalRotation = transform.rotation;
        lookingRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 180, 0)); // Rotate 180° on Y
        StartCoroutine(ControlRoutine());
    }

    private IEnumerator ControlRoutine()
    {
        while (true)
        {
            // Not looking at player
            isLookingAtPlayers = false;
            transform.rotation = originalRotation;

            if (!musicSource.isPlaying)
                musicSource.Play();

            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Turn to look at players
            isLookingAtPlayers = true;
            transform.rotation = lookingRotation;

            if (musicSource.isPlaying)
                musicSource.Stop();

            float lookTime = Random.Range(minLookTime, maxLookTime);
            yield return new WaitForSeconds(lookTime);
        }
    }
}
