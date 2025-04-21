using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    private Door _door;
    [SerializeField] private GameObject timeline;
    
    private void Start()
    {
        StartCoroutine(FindDoorCE());
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindObjectOfType<Door>();
        _door.isLocked = true;
    }

    public void UnlockDoor()
    {
        timeline.SetActive(true);
        _door.isLocked = false;
    }
}
