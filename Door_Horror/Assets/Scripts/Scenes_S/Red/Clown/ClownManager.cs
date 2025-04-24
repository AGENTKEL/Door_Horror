using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownManager : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;

    public float minLookTime = 3f;
    public float maxLookTime = 5f;

    public float rotationSpeed = 90f; // degrees per second
    public AudioSource musicSource;

    private bool isLookingAtPlayers = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private bool rotating = false;

    private void Start()
    {
        originalRotation = transform.rotation;
        StartCoroutine(ControlRoutine());
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

                // Toggle music based on the new rotation state
                if (isLookingAtPlayers)
                {
                    musicSource.Stop();
                }
                else
                {
                    musicSource.Play();
                }
            }
        }
    }

    private IEnumerator ControlRoutine()
    {
        while (true)
        {
            // Not looking at players
            isLookingAtPlayers = false;
            RotateTo(originalRotation);
            yield return new WaitUntil(() => !rotating);

            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Looking at players
            isLookingAtPlayers = true;
            Quaternion lookRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 180, 0));
            RotateTo(lookRotation);
            yield return new WaitUntil(() => !rotating);

            float lookTime = Random.Range(minLookTime, maxLookTime);
            yield return new WaitForSeconds(lookTime);
        }
    }

    private void RotateTo(Quaternion target)
    {
        targetRotation = target;
        rotating = true;
    }
}
