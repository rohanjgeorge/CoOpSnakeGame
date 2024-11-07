using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    public static GameOverController Instance; // Singleton instance for easy access

    private void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver(string winner, int points)
    {
        // Stop the game logic
        Time.timeScale = 0f;

        // Show the Game Over UI
        gameOverPanel.SetActive(true);
        gameOverText.text = $"{winner} won with {points} points!";

        // Set up button listeners
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // Resume time
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainLevel");
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
