using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    MainMenu menuScript;

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
    }
}
