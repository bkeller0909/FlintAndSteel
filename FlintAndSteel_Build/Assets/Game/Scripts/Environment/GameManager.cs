using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Coins")]
    public int coinAmount = 0;
    public int coinsAtLevelStart = 0;

    [Header("Controller")]
    public bool usingController;
    GameObject crosshair;

    private void Awake()
    {
        crosshair = GameObject.FindGameObjectWithTag("Cursor");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Anopther copy of the coin counter exists. Destroying this copy");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        registeredForReset = new List<ResetBehaviour>();
    }

    private void Update()
    {
        if(crosshair != null) 
        {
            if (!usingController)
            {
                crosshair.SetActive(false);
            }
            else
            {
                crosshair.SetActive(true);
            }
        }
    }

    public void IncrementCoinCount()
    {
        coinAmount++;
    }

    private List<ResetBehaviour> registeredForReset;

    public static void RegisterForReset(ResetBehaviour resetBehaviour)
    {
        Instance.registeredForReset.Add(resetBehaviour);
    }
    
    public static void GameReset()
    {
        for(int i =0; i < instance.registeredForReset.Count; i ++)
        {
            Instance.registeredForReset[i].Reset();
        }
    }
}
