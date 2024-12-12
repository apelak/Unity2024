using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame(float moveInterval)
    {
        PlayerPrefs.SetFloat("MoveInterval", moveInterval);
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }
}
