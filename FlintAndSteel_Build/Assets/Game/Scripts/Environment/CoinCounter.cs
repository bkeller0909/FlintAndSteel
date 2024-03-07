using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public CoinCounter Instance;

    public TextMeshProUGUI coinCounterText;
    public int coinAmount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Anopther copy of the coin counter exists. Destroying this copy");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //Updating the text in real time to display the current coin count
        if (coinCounterText != null)
        {
            coinCounterText.text = coinAmount.ToString();
        }
    }

    public void IncrementCoinCount()
    {
        coinAmount++;
    }
}
