using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject FadeOut;

    public void PlayGame()
    {
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
}
