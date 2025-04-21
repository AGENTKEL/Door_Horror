using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{
    [Header("Toy Audio")]
    public AudioSource audioSource1; // No gift
    public AudioSource audioSource2; // With gift

    [Header("Key To Spawn")]
    public GameObject keyPrefab; // Only on the last toy
    public bool isLastToy = false;

    private bool isGifted = false;
    [SerializeField] private ToyManager toyManager;
    

    public void Interact(Inventory inventory)
    {
        if (isGifted) return;

        if (inventory.HasItem(ItemType.Gift)) // assuming "Gift" is a Tool
        {
            inventory.UseItem(ItemType.Gift);
            audioSource2?.Play();
            isGifted = true;

            toyManager?.OnToyGifted(this);

            if (isLastToy && keyPrefab != null)
            {
                Instantiate(keyPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);
            }
        }
        else
        {
            audioSource1?.Play();
        }
    }
}
