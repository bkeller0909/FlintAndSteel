using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    bool isPaused = false;
    public AudioMixer audioMixer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause(!isPaused);
        }    
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        pauseCanvas.SetActive(paused);

        if (isPaused)
        {
            Debug.Log("The game is paused");
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("The game is no longer paused");
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
}
