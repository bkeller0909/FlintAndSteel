using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject flyingBomber;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private float spawnInterval = 30f;

    private GameObject currentParrot; // Reference to the currently spawned parrot


    [SerializeField] private Renderer[] parrotRenderer;

    private float fadeOutValue = 0f; // Initial fade out value
    private float fadeOutSpeed = 1.0f; // Speed of fade out

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Check if there's already a parrot alive
            if (currentParrot == null)
            {
                // Spawn a new parrot
                int spawnIndex = Random.Range(0, enemySpawnPoints.Length);
                currentParrot = Instantiate(flyingBomber, enemySpawnPoints[spawnIndex].position, Quaternion.identity);
            }
        }
    }

    // This method is called externally to inform the manager that a parrot has died
    public void ParrotDied()
    {
        currentParrot = null; // Reset the current parrot reference
    }

    public void SetFadeOutValue(float value)
    {
        foreach (Renderer renderer in parrotRenderer)
        {
            renderer.material.SetFloat("_FadeOut", value);
        }
    }
}
