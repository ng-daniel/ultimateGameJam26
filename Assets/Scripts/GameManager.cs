using UnityEngine;

public class GameManager : MonoBehaviour
{
    float timeMs;
    bool gameEnded = false;

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

    void Update()
    {
        if (!gameEnded)
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
        if (uiManager != null)
        {
            uiManager.ShowWinScreen();
        }
    }
}
