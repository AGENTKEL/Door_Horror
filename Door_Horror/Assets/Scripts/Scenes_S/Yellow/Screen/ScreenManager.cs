using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Sprite[] screenSprites;
    public Material screenMaterial;
    public float changeInterval = 3f;
    
    public AudioClip doorUnlockClip; // 🔥 New: unlock sound clip
    [SerializeField] private AudioSource audioSource; // 🔥 New: audio source to play sound

    private Door _door;
    private DoorBlack _doorBlack;
    private float timer = 0f;
    private bool isLooking = false;
    private bool doorUnlocked = false;

    private int switchesCount = 0;
    private int switchesNeeded = 5;
    private List<Sprite> availableSprites = new List<Sprite>();

    private void Start()
    {
        StartCoroutine(FindDoorCE());

        // Start with no image
        screenMaterial.mainTexture = null;

        // Initialize available sprites list
        availableSprites = new List<Sprite>(screenSprites);
        
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);

        _door = FindObjectOfType<Door>();
        _doorBlack = FindObjectOfType<DoorBlack>();
        if (_door != null)
        {
            _door.isLocked = true;
        }
    }

    private void Update()
    {
        if (!isLooking || doorUnlocked) return;
        
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;

            if (availableSprites.Count > 0)
            {
                // Pick random sprite
                int randomIndex = Random.Range(0, availableSprites.Count);
                Sprite sprite = availableSprites[randomIndex];

                if (sprite != null && sprite.texture != null)
                {
                    screenMaterial.mainTexture = sprite.texture;
                }

                // Remove used sprite so it doesn't repeat
                availableSprites.RemoveAt(randomIndex);

                switchesCount++;

                if (switchesCount >= switchesNeeded)
                {
                    UnlockDoor();
                }
            }
            else
            {
                Debug.Log("No more available sprites to switch.");
            }
        }
    }

    private void UnlockDoor()
    {
        doorUnlocked = true;

        if (_door != null)
            _door.isLocked = false;
        _doorBlack.UnlockBlackDoor();

        if (audioSource != null && doorUnlockClip != null)
            audioSource.PlayOneShot(doorUnlockClip);

        Debug.Log("Door unlocked after switching 5 images!");
    }

    public void SetLooking(bool value)
    {
        if (isLooking != value)
        {
            isLooking = value;

            if (!isLooking)
            {
                timer = 0f;
            }
        }
    }
}
