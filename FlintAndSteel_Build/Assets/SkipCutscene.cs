using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    MainMenu menuScript;
    [SerializeField] GameObject skipText;

    private void Awake()
    {
        menuScript = GetComponent<MainMenu>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dialogue Skip") || Input.GetKeyDown(KeyCode.E))
        {
            menuScript.LoadLevel();
        }

        if(GameManager.Instance.usingController == true)
        {
            skipText.GetComponent<TextMeshProUGUI>().text = "Skip: SQR";
        }
        else
        {
            skipText.GetComponent<TextMeshProUGUI>().text = "Skip: E";
        }
    }
}
