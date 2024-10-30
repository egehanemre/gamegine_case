using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance;
    public TMP_Text coinText;
    public int coinCount;
    public static int Score;

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