using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] private GameObject pauseFirstButton;

    bool isPaused = false;
    public AudioMixer audioMixer;

    [SerializeField] GameObject playerGO;


    [Header("Controller")]
    [SerializeField] Toggle controllerToggle;
    public bool controllerON = false;
    /*GameObject crosshair;*/

    private void Awake()
    {
        if (GameManager.Instance.usingController == true)
        {
            controllerToggle.isOn = true;
            controllerON = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(!isPaused);
        }

        playerGO.GetComponent<Throwing>().enabled = !isPaused;
        playerGO.GetComponent<PlayerAttackScript>().enabled = !isPaused;
        playerGO.GetComponent<PlayerMove>().enabled = !isPaused;
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        pauseCanvas.SetActive(paused);
        gameCanvas.SetActive(!paused);

        if (isPaused)
        {
            //Debug.Log("The game is paused");
            Time.timeScale = 0;

            // clear selected button objects
            EventSystem.current.SetSelectedGameObject(null);

            // set new selected button
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        else
        {
            //Debug.Log("The game is no longer paused");
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Adjusts the volume within the pause menu.
    /// </summary>
    /// <param name="volume">Value of the slider in the menu</param>
    public void AdjustVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }

    // Quits to the main menu
    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("IntroCutscene");
    }

    // Restarts level and sets coins to the amount the player had at the start of the level
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.coinAmount = GameManager.Instance.coinsAtLevelStart;
    }

    public void Controller(bool isController)
    {
        controllerON = isController;

        if (controllerON)
        {
            GameManager.Instance.usingController = true;
            Cursor.visible = false;
        }
        else
        {
            GameManager.Instance.usingController = false;
            Cursor.visible = true;
        }
    }
}
