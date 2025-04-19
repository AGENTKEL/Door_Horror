using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Key, Note, Tool }

public class Interactable : MonoBehaviour
{
    public ItemType itemType;
    public Sprite itemSprite;
}
