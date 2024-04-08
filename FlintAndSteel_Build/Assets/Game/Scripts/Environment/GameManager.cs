using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    /*[SerializeField] public Toggle controllerToggle;*/

    [Header("Coins")]
    public int coinAmount = 0;
    public int coinsAtLevelStart = 0;

    [Header("Controller")]
    public bool usingController = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another copy of the coin counter exists. Destroying this copy");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        /*if(usingController == true)
        {
            controllerToggle.enabled = true;
        }*/

        registeredForReset = new List<ResetBehaviour>();
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

    /*public void Controller(bool isController)
    {
        usingController = isController;

        crosshair = GameObject.FindGameObjectWithTag("Cursor");
        if (crosshair != null)
        {
            if (usingController)
            {
                crosshair.SetActive(true);
                PlayerPrefs.Save();
            }
            else
            {
                crosshair.SetActive(false);
                PlayerPrefs.Save();
            }
        }
    }*/
}
