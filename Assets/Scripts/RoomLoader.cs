using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    [SerializeField] RoomQueue roomQueue;
    UIManager uiManager;
    GameManager gameManager;

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
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found in the scene.");
        }
    }

    private void LoadRoom(RoomConfig config)
    {
        Debug.Log("Loading room with config: " + config.name);
        Instantiate(config.Prefab, Vector3.zero, Quaternion.identity);
        uiManager.SetLevelName(config.Name);
    }

    public void TryBackToMainMenu()
    {
        if (gameManager != null && gameManager.IsGameOver())
        {
            Debug.Log("Returning to main menu...");
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void TryRestartLevel()
    {
        if (gameManager != null && gameManager.IsGameOver())
        {
            Debug.Log("Restarting level...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }   
    }
}
