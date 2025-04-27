using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLevelManager : MonoBehaviour
{
    [Header("List of Red Room Prefabs (disabled in scene)")]
    public List<GameObject> redRoomPrefabs;

    private void Start()
    {
        ActivateRandomRedLevel();
    }

    void ActivateRandomRedLevel()
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < redRoomPrefabs.Count; i++)
        {
            if (!Game_Manager.instance.redRoomsUsed.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("No more unique Yellow levels left. Using fallback (first prefab).");
            redRoomPrefabs[0].SetActive(true);
            return;
        }

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        redRoomPrefabs[randomIndex].SetActive(true);

        // Track as used
        Game_Manager.instance.redRoomsUsed.Add(randomIndex);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
    }
}
