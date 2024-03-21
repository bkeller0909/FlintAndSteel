using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Build Index of the Level you want to Load")]
    [SerializeField] private int nextLevelIndex;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Loads the next level when the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Load", true);
        }
    }

    public void LoadLevel()
    {
        GameManager.Instance.coinsAtLevelStart = GameManager.Instance.coinAmount;
        SceneManager.LoadScene(nextLevelIndex);
    }
}
