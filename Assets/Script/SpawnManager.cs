using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private Canvas healthBarCanvas;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        WaitForSecondsRealtime spawnDelay = new WaitForSecondsRealtime(spawnInterval);

        while (true)
        {
            yield return spawnDelay;

            int randomSpawnIndex = Random.Range(0, spawnLocations.Length);
            Transform spawnLocation = spawnLocations[randomSpawnIndex];

            GameObject agent = Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);
            //agent.AddComponent<EnemyManager>().SetupHealthBar(healthBarCanvas,_camera);
        }
    }
}
