using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject FadeOut;

    [Header("Controller")]
    [SerializeField] Toggle controllerToggle;
    public bool controllerON = false;

    private void Awake()
    {
        if (GameManager.Instance.usingController == true)
        {
            controllerToggle.isOn = true;
            controllerON = true;
        }
    }

    public void PlayGame()
    {
        if (FadeOut != null)
            FadeOut.SetActive(true);   
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        GameManager.Instance.coinAmount = 0;
        GameManager.Instance.coinsAtLevelStart = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Controller(bool isController)
    {
        controllerON = isController;

        if (controllerON)
        {
            GameManager.Instance.usingController = true;
        }
        else
        {
            GameManager.Instance.usingController = false;
        }
    }
}
