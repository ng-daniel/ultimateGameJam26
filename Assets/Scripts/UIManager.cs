using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject playScreen;
    
    [SerializeField] string timeText;
    [SerializeField] TMP_Text gameTimeText;
    [SerializeField] TMP_Text finalTimeText;
    [SerializeField] string levelNameText;
    [SerializeField] TMP_Text gameLevelNameText;
    [SerializeField] TMP_Text finalLevelNameText;

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
        if (playScreen != null)
        {
            playScreen.SetActive(false);
        }
    }

    public void ShowPlayScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
        if (playScreen != null)
        {
            playScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Formats time into minutes, seconds, and milliseconds as 00:00:000 and updates the time text UI element.
    /// </summary>
    /// <param name="timeMillis">The time in milliseconds as a float.</param>
    public void UpdateTimeText(float timeMillis)
    {
        float timeSec = timeMillis / 1000f;
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(timeSec / 60f);
            int seconds = Mathf.FloorToInt(timeSec % 60f);
            int milliseconds = Mathf.FloorToInt((timeSec * 1000f) % 1000f);
            timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            if (gameTimeText != null)
            {
                gameTimeText.text = timeText;
            }
            if (finalTimeText != null)
            {
                finalTimeText.text = timeText;
            }
        }
    }

    public void SetLevelName(string levelName)
    {
        levelNameText = levelName;
        
        if (gameLevelNameText != null)
        {
            gameLevelNameText.text = levelNameText;
        }
        if (finalLevelNameText != null)
        {
            finalLevelNameText.text = levelNameText;
        }
    }
}
