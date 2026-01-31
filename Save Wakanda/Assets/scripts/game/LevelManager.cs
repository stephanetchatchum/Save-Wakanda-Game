using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages level progression and level selection.
/// Handles loading different levels and tracking progress.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Level Configuration")]
    [Tooltip("Add your level data assets here in order (Level 1, 2, 3)")]
    public List<LevelData> levels = new List<LevelData>();
    
    [Header("References")]
    public PuzzleManager puzzleManager;
    
    [Header("UI References")]
    public GameObject levelSelectPanel;
    public Transform levelButtonContainer;
    public GameObject levelButtonPrefab;
    
    private int currentLevelIndex = 0;

    void Start()
    {
        if (levels.Count == 0)
        {
            Debug.LogError("No levels assigned to LevelManager!");
            return;
        }
        
        CreateLevelSelectButtons();
        LoadLevel(0);
    }

    /// <summary>
    /// Create buttons for level selection
    /// </summary>
    void CreateLevelSelectButtons()
    {
        if (levelButtonContainer == null || levelButtonPrefab == null) return;
        
        for (int i = 0; i < levels.Count; i++)
        {
            int levelIndex = i; // Capture for lambda
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
            
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            
            if (buttonText != null)
            {
                buttonText.text = $"Level {levels[i].levelNumber}\n{levels[i].levelName}";
            }
            
            if (button != null)
            {
                button.onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
            }
        }
    }

    /// <summary>
    /// Called when a level selection button is clicked
    /// </summary>
    void OnLevelButtonClicked(int levelIndex)
    {
        LoadLevel(levelIndex);
        
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Load a specific level by index
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
            return;
        }
        
        currentLevelIndex = levelIndex;
        LevelData levelToLoad = levels[levelIndex];
        
        if (puzzleManager != null)
        {
            puzzleManager.LoadLevel(levelToLoad);
        }
        
        Debug.Log($"Loaded Level {levelToLoad.levelNumber}: {levelToLoad.levelName}");
    }

    /// <summary>
    /// Load the next level in sequence
    /// </summary>
    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        
        if (nextIndex < levels.Count)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
            // You could show a "You Win!" screen here
        }
    }

    /// <summary>
    /// Load the previous level
    /// </summary>
    public void LoadPreviousLevel()
    {
        int prevIndex = currentLevelIndex - 1;
        
        if (prevIndex >= 0)
        {
            LoadLevel(prevIndex);
        }
    }

    /// <summary>
    /// Restart the current level
    /// </summary>
    public void RestartCurrentLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    /// <summary>
    /// Show the level selection panel
    /// </summary>
    public void ShowLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hide the level selection panel
    /// </summary>
    public void HideLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Get the current level data
    /// </summary>
    public LevelData GetCurrentLevel()
    {
        if (currentLevelIndex >= 0 && currentLevelIndex < levels.Count)
        {
            return levels[currentLevelIndex];
        }
        return null;
    }

    /// <summary>
    /// Get total number of levels
    /// </summary>
    public int GetTotalLevels()
    {
        return levels.Count;
    }
}
