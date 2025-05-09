using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackLevelManager : MonoBehaviour
{
    [Header("List of Black Room Prefabs (disabled in scene)")]
    public List<GameObject> blackRoomPrefabs;

    private void Start()
    {
        ActivateNextBlackLevel();
    }

    void ActivateNextBlackLevel()
    {
        int blackRoomCount = Game_Manager.instance.blackRoomsPassed;

        if (blackRoomPrefabs.Count == 0)
        {
            Debug.LogError("No black room prefabs assigned!");
            return;
        }

        int indexToActivate = Mathf.Clamp(blackRoomCount, 0, blackRoomPrefabs.Count - 1);
        blackRoomPrefabs[indexToActivate].SetActive(true);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
    }
}
