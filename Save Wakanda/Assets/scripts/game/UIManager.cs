using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all UI elements and button interactions.
/// Handles menus, win screens, and game controls.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Manager References")]
    public PuzzleManager puzzleManager;
    public LevelManager levelManager;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("Buttons")]
    public Button shuffleButton;
    public Button resetButton;
    public Button pauseButton;
    public Button nextLevelButton;
    public Button mainMenuButton;

    [Header("Win Panel")]
    public Text winMovesText;
    public Text winLevelText;

    private bool isPaused = false;

    void Start()
    {
        SetupButtonListeners();
        ShowMainMenu();
    }

    /// <summary>
    /// Setup all button click listeners
    /// </summary>
    void SetupButtonListeners()
    {
        if (shuffleButton != null)
            shuffleButton.onClick.AddListener(OnShuffleClicked);
        
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetClicked);
        
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);
        
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    /// <summary>
    /// Show the main menu
    /// </summary>
    public void ShowMainMenu()
    {
        SetActivePanel(mainMenuPanel);
    }

    /// <summary>
    /// Show the game panel
    /// </summary>
    public void ShowGame()
    {
        SetActivePanel(gamePanel);
    }

    /// <summary>
    /// Show the win panel
    /// </summary>
    public void ShowWinPanel(int moves, string levelName)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            
            if (winMovesText != null)
                winMovesText.text = $"Completed in {moves} moves!";
            
            if (winLevelText != null)
                winLevelText.text = $"Level: {levelName}";
        }
    }

    /// <summary>
    /// Hide the win panel
    /// </summary>
    public void HideWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Set only one panel active
    /// </summary>
    void SetActivePanel(GameObject panel)
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(panel == mainMenuPanel);
        if (gamePanel != null) gamePanel.SetActive(panel == gamePanel);
        if (pausePanel != null) pausePanel.SetActive(panel == pausePanel);
    }

    // Button Click Handlers

    void OnShuffleClicked()
    {
        if (puzzleManager != null)
        {
            puzzleManager.ShufflePuzzle();
        }
    }

    void OnResetClicked()
    {
        if (puzzleManager != null)
        {
            puzzleManager.ResetPuzzle();
        }
    }

    void OnPauseClicked()
    {
        isPaused = !isPaused;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
        
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void OnNextLevelClicked()
    {
        HideWinPanel();
        
        if (levelManager != null)
        {
            levelManager.LoadNextLevel();
        }
    }

    void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
    }

    /// <summary>
    /// Start game with selected level
    /// </summary>
    public void StartGame(int levelIndex = 0)
    {
        ShowGame();
        
        if (levelManager != null)
        {
            levelManager.LoadLevel(levelIndex);
        }
    }

    /// <summary>
    /// Quit application
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// Reload current scene
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
