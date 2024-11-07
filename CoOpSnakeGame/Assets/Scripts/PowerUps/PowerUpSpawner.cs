using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shieldPowerUpPrefab;
    [SerializeField] private GameObject scoreBoostPowerUpPrefab;
    [SerializeField] private GameObject speedUpPowerUpPrefab;
    [SerializeField] private float spawnInterval = 10f; // Time between spawns
    [SerializeField] private float despawnTime = 5f;    // Power-up duration before disappearing
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomPowerUp();
            timer = 0f;
        }
    }

    private void SpawnRandomPowerUp()
    {
        // Calculate the screen bounds
        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Generate random positions within the game window size
        float xPosition = Random.Range(screenLeft, screenRight);
        float yPosition = Random.Range(screenBottom, screenTop);
        Vector2 spawnPosition = new Vector2(xPosition, yPosition);

        // Randomly select a power-up to spawn
        GameObject selectedPowerUp;
        int randomPowerUp = Random.Range(0, 3);

        if (randomPowerUp == 0)
            selectedPowerUp = shieldPowerUpPrefab;
        else if (randomPowerUp == 1)
            selectedPowerUp = scoreBoostPowerUpPrefab;
        else
            selectedPowerUp = speedUpPowerUpPrefab;

        // Instantiate the power-up and set it to despawn after a set time
        GameObject spawnedPowerUp = Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);
        Destroy(spawnedPowerUp, despawnTime);
    }
}
