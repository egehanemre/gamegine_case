namespace ManagementScripts
{
    using TMPro;
    using UnityEngine;

    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance;
        public static int Score;

        public TMP_Text coinText;
        public int coinCount;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            coinText.text = coinCount.ToString();
        }

        public void GetCoins(int coinValue)
        {
            coinCount += coinValue;
            Score += coinValue;
            coinText.text = coinCount.ToString();
        }

        public void SpendCoins(int coinValue)
        {
            coinCount -= coinValue;
            coinText.text = coinCount.ToString();
        }
    }
}
