using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        registeredForReset = new List<ResetBehaviour>();
    }

    private void Update()
    {
        KillRestartLevel();
        KillToMenu();
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

    // kills game to the main menu
    public void KillToMenu()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("IntroCutscene");
        }
    }

    // kills level and sets coins to the amount the player had at the start of the level
    public void KillRestartLevel()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            coinAmount = coinsAtLevelStart;
        }
    }
}
