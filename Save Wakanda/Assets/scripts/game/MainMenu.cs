using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuUI;       // Canvas or Panel
    public string gameSceneName = "GameScene";

    private bool isMenuOpen = false;

    void Start()
    {
        menuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuUI.SetActive(isMenuOpen);

        // Pause / Resume game
        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
