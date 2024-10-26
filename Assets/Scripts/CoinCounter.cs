using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    
    public TMP_Text coinText;
    public int coinCount = 0;

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        coinText.text = coinCount.ToString();
    }

    public void GetCoins(int coinValue)
    {
        coinCount += coinValue;
        coinText.text = coinCount.ToString();
    }
}
