using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public TMPro.TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        LoadGameOverScene();
    }

    public void TryAgain()
    {
        LoadGameScene();
        Time.timeScale = 1;
    }

    private void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private void LoadGameOverScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }

    public void DisplayScore()
    {
        scoreText.text = "Score: " + CoinCounter.Score;
    }

    public void SetScoreText(TMPro.TextMeshProUGUI newScoreText)
    {
        scoreText = newScoreText;
    }
}

