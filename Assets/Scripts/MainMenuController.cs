using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject GameWinCanvas;
    public bool isPaused = false;
    public void SelectScene(string sceneName)
    {
        UnPause();
        SceneManager.LoadScene(sceneName);
    }
    public void Restart()
    {
        UnPause();
        string currentLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevelName);

    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        UnPause();
        SceneManager.LoadScene("MainMenu");
    }
    public void PauseGame()
    {
        if (!isPaused)
        {
            Pause();
            PauseCanvas.SetActive(true);
        }
    }
    public void ResumeGame()
    {
        if (isPaused)
        {
            UnPause();
            PauseCanvas.SetActive(false);
        }
    }
    public void GameOverMenu(bool state)
    {
        GameOverCanvas.SetActive(state);
        if (state)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }
    public void GameWinMenu(bool state)
    {
        GameWinCanvas.SetActive(state);
        if (state)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }
    private void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }
    private void UnPause()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}
