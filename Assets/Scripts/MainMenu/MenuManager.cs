using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] RoomQueue roomQueue;

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject levelSelectUI;
    public void PlayLevel(RoomConfig config)
    {
        if (roomQueue != null)
        {
            roomQueue.config = config;
            Debug.Log("Assigned RoomConfig to RoomQueue: " + config.name);
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            Debug.LogError("RoomQueue not found in the scene. Please ensure a RoomQueue ScriptableObject is present.");
        }
    }

    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        levelSelectUI.SetActive(false);
    }
    public void ShowLevelSelect()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        ShowMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
