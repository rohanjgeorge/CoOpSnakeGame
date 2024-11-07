using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject massGainerPrefab;

    [SerializeField]
    private GameObject massBurnerPrefab;

    [SerializeField]
    private float spawnInterval = 5f;

    [SerializeField]
    private float despawnTime = 8f; // Time before food despawns if uneaten
    private float timer;

    private SnakeControllerOne snakeOneController;
    private SnakeControllerTwo snakeTwoController;

private void Start()
    {
        // Find both SnakeController and SnakeControllerB in the scene
        snakeOneController = FindObjectOfType<SnakeControllerOne>();
        snakeTwoController = FindObjectOfType<SnakeControllerTwo>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Spawn food at regular intervals
        if (timer >= spawnInterval)
        {
            SpawnRandomFood();
            timer = 0f;
        }
    }

    private void SpawnRandomFood()
    {
        // Choose randomly between Mass Gainer and Mass Burner
        GameObject selectedFood = Random.Range(0, 2) == 0 ? massGainerPrefab : massBurnerPrefab;

        // Avoid spawning Mass Burner if snake is too small
        if (selectedFood == massBurnerPrefab && (snakeOneController.GetSnakeSize() <= 1 || snakeTwoController.GetSnakeSize() <= 1))
        {
            selectedFood = massGainerPrefab;
        }

        // Calculate the screen bounds
        float screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Generate random positions within the game window size
        float xPosition = Random.Range(screenLeft, screenRight);
        float yPosition = Random.Range(screenBottom, screenTop);
        Vector2 spawnPosition = new Vector2(xPosition, yPosition);

        // Instantiate food at the calculated position and destroy after despawnTime
        GameObject spawnedFood = Instantiate(selectedFood, spawnPosition, Quaternion.identity);
        Destroy(spawnedFood, despawnTime); // Auto-despawn after set time
    }
}
