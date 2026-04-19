using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    [SerializeField] RoomQueue roomQueue;
    UIManager uiManager;

    public void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not found in the scene.");
        }

        if (roomQueue == null)
        {
            Debug.LogError("RoomQueue is not assigned in RoomLoader.");
        }

        RoomConfig config = roomQueue.config;
        if (config == null)
        {
            Debug.LogError("RoomConfig is not assigned in RoomQueue.");
        }
        else
        {
            // Load the room using the config
            LoadRoom(config);
        }
    }

    private void LoadRoom(RoomConfig config)
    {
        Debug.Log("Loading room with config: " + config.name);
        Instantiate(config.Prefab, Vector3.zero, Quaternion.identity);
        uiManager.SetLevelName(config.name);
    }
}
