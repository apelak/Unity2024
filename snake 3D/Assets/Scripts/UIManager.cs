using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text fruitCounterText;
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
        UpdateFruitCounter(0);
    }

    public void UpdateFruitCounter(int count)
    {
        fruitCounterText.text = "Punkty: " + count.ToString();
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
