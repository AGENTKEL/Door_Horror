using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Sprite[] screenSprites;
    public Material screenMaterial;
    public float changeInterval = 3f;

    private Door _door;
    private int currentIndex = -1; // Start before the first sprite
    private float timer = 0f;
    private bool isLooking = false;
    private bool doorUnlocked = false;

    private void Start()
    {
        StartCoroutine(FindDoorCE());

        // Start with no image
        screenMaterial.mainTexture = null;
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindObjectOfType<Door>();
        if (_door != null)
        {
            _door.isLocked = true;
        }
    }

    private void Update()
    {
        if (!isLooking) return;
        
        if (isLooking)
        {
            timer += Time.deltaTime;

            if (timer >= changeInterval)
            {
                timer = 0f;

                if (currentIndex < screenSprites.Length - 1)
                {
                    currentIndex++;
                    var sprite = screenSprites[currentIndex];
                
                    if (sprite != null && sprite.texture != null)
                    {
                        screenMaterial.mainTexture = sprite.texture;
                    }

                    if (currentIndex == screenSprites.Length - 1 && !doorUnlocked)
                    {
                        doorUnlocked = true;
                        if (_door != null) _door.isLocked = false;
                        Debug.Log("Door unlocked after viewing last screen!");
                    }
                }
                else
                {
                    Debug.Log("At final index, not advancing.");
                }
            }
        }
    }

    public void SetLooking(bool value)
    {
        // Only do something if the value is actually changing
        if (isLooking != value)
        {
            isLooking = value;

            if (!isLooking)
            {
                // Reset only when we stop looking
                timer = 0f;
            }
        }
    }
}
