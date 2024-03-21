using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject credits;

    public void StartCredits()
    {
        credits.SetActive(true);
    }

}
