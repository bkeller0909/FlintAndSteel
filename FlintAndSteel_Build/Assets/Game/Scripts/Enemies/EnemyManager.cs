using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject flyingBomber;
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private float spawnInterval = 30f;

    private GameObject currentParrot; // Reference to the currently spawned parrot


    [SerializeField] private Material ParrotSpawn;

    [SerializeField] private float fadeInDuration = 1.0f; // Duration of fade-in effect


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

            StartCoroutine(FadeInParrot(currentParrot));
          
        }
    }

    private IEnumerator FadeInParrot(GameObject parrot)
    {
        MeshRenderer[] parrotRenderers = parrot.GetComponentsInChildren<MeshRenderer>();

        float elapsedTime = 0.0f;
        while (elapsedTime < fadeInDuration)
        {
            float fadeInValue = elapsedTime / fadeInDuration;
            foreach (MeshRenderer renderer in parrotRenderers)
            {
                renderer.material.SetFloat("_FadeOut", fadeInValue);
            }
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        // Ensure the final fade-in value is 1
        foreach (MeshRenderer renderer in parrotRenderers)
        {
            renderer.material.SetFloat("_FadeOut", 1f);
        }
    }

    // This method is called externally to inform the manager that a parrot has died
    public void ParrotDied()
    {
        currentParrot = null; // Reset the current parrot reference
    }
}
