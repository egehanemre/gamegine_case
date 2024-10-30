namespace ManagementScripts
{
    using UnityEngine;

    public class GameOverSceneManager : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI scoreText;

        private void Start()
        {
            SceneManager.Instance.SetScoreText(scoreText);
            SceneManager.Instance.DisplayScore();
        }
    }
}