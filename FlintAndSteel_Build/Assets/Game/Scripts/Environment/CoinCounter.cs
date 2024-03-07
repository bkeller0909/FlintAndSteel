using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    private CoinCounter instance;
    public CoinCounter Instance { get { return instance; } }

    public int coinAmount;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        coinAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = coinAmount.ToString();
    }
}
