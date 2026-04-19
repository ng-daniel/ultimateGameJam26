using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timeMs;
    bool gameEnded = false;
    bool gameStarted = true;

    UIManager uiManager;
    DirtyRoomManager dirtyRoomManager;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        dirtyRoomManager = FindFirstObjectByType<DirtyRoomManager>();
        if (uiManager != null)
        {
            uiManager.ShowPlayScreen();
        }
    }

    /// <summary>
    /// Call this method to start the game. It resets the timer and sets the game state to active.
    /// Called from PlayerSpawning after the player is initially spawned to ensure the timer starts when the game begins.
    /// The object that contains PlayerSpawning is created after RoomLoader has loaded the room from the RoomQueue.
    /// </summary>
    public void StartGame()
    {
        timeMs = 0f;
        gameEnded = false;
        gameStarted = true;
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            timeMs += Time.deltaTime * 1000f;
            uiManager.UpdateTimeText(timeMs);

            if (dirtyRoomManager.CheckIfFullyCleaned())
            {
                OnGameEnd();
            }
        }
    }
    public void OnGameEnd()
    {
        gameEnded = true;
        // dirtyRoomManager.CleanAllSurfaces(); // Ensure all surfaces are fully clean at the end of the game
        if (uiManager != null)
        {
            uiManager.ShowWinScreen();
        }
    }

    public bool IsGameOver()
    {
        return gameEnded;
    }
}
