using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manequin : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;
    public float checkInterval = 0.2f;

    private Camera playerCamera;
    private bool isVisible;

    [SerializeField] private Transform[] visibilityPoints;

    void Start()
    {
        playerCamera = Camera.main;
        StartCoroutine(CheckVisibilityRoutine());
    }

    void Update()
    {
        if (!isVisible)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
        }
    }

    private System.Collections.IEnumerator CheckVisibilityRoutine()
    {
        while (true)
        {
            isVisible = IsAnyPointVisible(); // Updated function name for clarity
            yield return new WaitForSeconds(checkInterval);
        }
    }

    bool IsAnyPointVisible()
    {
        foreach (Transform point in visibilityPoints)
        {
            Vector3 viewPos = playerCamera.WorldToViewportPoint(point.position);

            // Check if inside the screen (in front of camera and within viewport)
            if (viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
            {
                Vector3 direction = point.position - playerCamera.transform.position;
                Ray ray = new Ray(playerCamera.transform.position, direction);

                if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude + 0.1f))
                {
                    if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    {
                        return true; // At least one point is visible
                    }
                }
            }
        }

        return false; // None of the points are visible
    }
}
