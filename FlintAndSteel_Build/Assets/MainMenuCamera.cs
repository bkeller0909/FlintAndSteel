using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] GameObject MainMenuTitle;
    [SerializeField] GameObject[] Buttons;

    bool beginFade = false;
    float fadeValue;

    private void Start()
    {
        MainMenuTitle.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            Buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        }

        fadeValue = 0;
    }

    private void Update()
    {
        if (beginFade) 
        {
            Fade();
            fadeValue += Time.deltaTime;
        }
    }

    public void FadeInMenu()
    {
        beginFade = true;
        MainMenuTitle.SetActive(true);

        for (int i = 0; i < Buttons.Length; i++) 
        {
            Buttons[i].SetActive(true);
        }
    }

    private void Fade()
    {
        MainMenuTitle.GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Clamp01(fadeValue));

        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Clamp01(fadeValue));
            Buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, Mathf.Clamp01(fadeValue));
        }
    }
}
