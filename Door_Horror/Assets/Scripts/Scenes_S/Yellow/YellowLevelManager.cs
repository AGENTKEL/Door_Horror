using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowLevelManager : MonoBehaviour
{
    [Header("List of Yellow Room Prefabs (disabled in scene)")]
    public List<GameObject> yellowRoomPrefabs;

    private void Start()
    {
        ActivateRandomYellowLevel();
    }

    void ActivateRandomYellowLevel()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < yellowRoomPrefabs.Count; i++)
        {
            if (!Game_Manager.instance.yellowRoomsUsed.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("No more unique Yellow levels left. Using fallback (first prefab).");
            yellowRoomPrefabs[0].SetActive(true);
            return;
        }

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        yellowRoomPrefabs[randomIndex].SetActive(true);

        // Track as used
        Game_Manager.instance.yellowRoomsUsed.Add(randomIndex);
    }
}
