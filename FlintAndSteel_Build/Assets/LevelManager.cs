using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Build Index of the Level you want to Load")]
    [SerializeField] private int nextLevelIndex;

    // Loads the next level when the player enters the collider
    // We can add more things such as a delay or transition anim later on - Evan
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
}
