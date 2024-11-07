using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControllerTwo : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f; // Controls the snake's movement speed
    private float moveTimer;

    private Vector2 direction = Vector2.right; // Initial direction to the right
    private Vector2 nextDirection = Vector2.right; // Buffer to store the next turn

    [SerializeField]
    private GameObject bodySegmentPrefab;
    private List<Transform> bodySegments = new List<Transform>(); // List to hold body segments

    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    // To keep track of positions
    private Queue<Vector2> positions = new Queue<Vector2>();
    private Queue<Vector2> turnPositions = new Queue<Vector2>(); // Track turning points
    private Queue<Vector2> turnDirections = new Queue<Vector2>(); // Track corresponding directions
    private Queue<float> turnRotations = new Queue<float>(); // Store rotation angles at each turn

    // Size of the sprite for proper positioning
    private float segmentSize;

    // Power-Up Variables
    private bool shieldActive = false;
    private float scoreMultiplier = 1f;
    private float speedMultiplier = 1f;

    [SerializeField]
    private float powerUpDuration = 3f; // Duration of power-ups in seconds

    // Start is called before the first frame update
    void Start()
    {
        screenLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        screenRight = Camera.main.ViewportToWorldPoint(Vector3.right).x;
        screenBottom = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        screenTop = Camera.main.ViewportToWorldPoint(Vector3.up).y;

        if (bodySegmentPrefab.TryGetComponent<SpriteRenderer>(out var renderer))
        {
            segmentSize = renderer.bounds.size.x;
        }
        moveTimer = moveSpeed;

        // Add the initial position of the head to the queue to avoid gaps
        positions.Enqueue(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Set up WASD controls for Snake Two
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            nextDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            nextDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            nextDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            nextDirection = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        moveTimer -= Time.fixedDeltaTime * speedMultiplier;

        if (moveTimer <= 0f)
        {
            if (nextDirection != direction)
            {
                turnPositions.Enqueue(transform.position); // Store the position of the turn
                turnDirections.Enqueue(nextDirection); // Store the new direction at this turn
                turnRotations.Enqueue(CalculateRotationAngle(nextDirection)); // Store rotation

                direction = nextDirection;
            }

            MoveSnake();
            WrapPosition();
            moveTimer = moveSpeed; // Reset the timer
        }
    }

    private void WrapPosition()
    {
        // Check if the snake head is out of bounds and wrap it
        Vector3 newPosition = transform.position;

        if (transform.position.x < screenLeft)
        {
            newPosition.x = screenRight;
        }
        else if (transform.position.x > screenRight)
        {
            newPosition.x = screenLeft;
        }

        if (transform.position.y < screenBottom)
        {
            newPosition.y = screenTop;
        }
        else if (transform.position.y > screenTop)
        {
            newPosition.y = screenBottom;
        }

        transform.position = newPosition;
    }

    private void MoveSnake()
    {
        Vector2 previousPosition = transform.position;
        transform.position = (Vector2)transform.position + direction * segmentSize;

        // Enqueue the new position of the head
        positions.Enqueue(transform.position);

        // Rotate the head to face the direction of movement
        RotateTowardsDirection(transform, CalculateRotationAngle(direction));

        // Limit the queue to the number of body segments
        while (positions.Count > bodySegments.Count + 1)
        {
            positions.Dequeue();
        }

        // Move each body segment to follow the previous segment's position
        for (int i = 0; i < bodySegments.Count; i++)
        {
            Vector2 tempPosition = bodySegments[i].position;
            bodySegments[i].position = positions.ToArray()[i];
            previousPosition = tempPosition;

            // Check if the segment has reached a turn point
            if (
                turnPositions.Count > 0
                && (Vector2)bodySegments[i].position == turnPositions.Peek()
            )
            {
                // Apply the rotation at the turn point
                RotateTowardsDirection(bodySegments[i], turnRotations.Peek());

                // Remove the turn point after the last segment passes it
                if (i == bodySegments.Count - 1)
                {
                    turnPositions.Dequeue();
                    turnDirections.Dequeue();
                    turnRotations.Dequeue();
                }
            }
            else
            {
                // Keep segment rotation consistent with the current direction between turns
                RotateTowardsDirection(bodySegments[i], CalculateRotationAngle(direction));
            }
        }
    }

    private float CalculateRotationAngle(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
    }

    private void RotateTowardsDirection(Transform segment, float angle)
    {
        segment.rotation = Quaternion.Euler(0, 0, angle);
    }

    [ContextMenu("Grow Snake")]
    public void GrowSnake()
    {
        Vector2 spawnPosition;

        if (bodySegments.Count == 0)
        {
            // Position the first segment directly behind the head
            spawnPosition = (Vector2)transform.position - direction * segmentSize;
        }
        else
        {
            // Position new segments at the end of the last segment
            Transform lastSegment = bodySegments[bodySegments.Count - 1];
            spawnPosition =
                (Vector2)lastSegment.position - (Vector2)(lastSegment.right * segmentSize);
        }

        GameObject newSegment = Instantiate(bodySegmentPrefab, spawnPosition, Quaternion.identity);
        bodySegments.Add(newSegment.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BodySegment"))
        {
            if (!shieldActive) {
                Die();
            }
        }
        else if (collision.CompareTag("MassGainer"))
        {
            Destroy(collision.gameObject);
            GrowSnake();
            IncreaseScore(10 * (int)scoreMultiplier);
        }
        else if (collision.CompareTag("MassBurner"))
        {
            Destroy(collision.gameObject);
            if (bodySegments.Count > 1) ShrinkSnake();
            IncreaseScore(-5);
        }
        else if (collision.CompareTag("ShieldPowerUp"))
        {
            ActivateShield();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("ScoreBoostPowerUp"))
        {
            ActivateScoreBoost();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("SpeedUpPowerUp"))
        {
            ActivateSpeedBoost();
            Destroy(collision.gameObject);
        }
    }

    private void ShrinkSnake()
    {
        if (bodySegments.Count > 0)
        {
            Transform lastSegment = bodySegments[bodySegments.Count - 1];
            bodySegments.RemoveAt(bodySegments.Count - 1);
            Destroy(lastSegment.gameObject);
        }
    }

    private void Die()
    {
        Debug.Log("Snake has died!");
        // Logic to reset the game or display a death screen here
    }

    public int GetSnakeSize()
    {
        return bodySegments.Count;
    }

    private void ActivateShield()
    {
        shieldActive = true;
        Invoke("DeactivateShield", powerUpDuration);
    }

    private void DeactivateShield()
    {
        shieldActive = false;
    }

    private void ActivateScoreBoost()
    {
        scoreMultiplier = 2f;
        Invoke("ResetScoreMultiplier", powerUpDuration);
    }

    private void ResetScoreMultiplier()
    {
        scoreMultiplier = 1f;
    }

    private void ActivateSpeedBoost()
    {
        speedMultiplier = 1.5f; // Increase speed by 50%
        Invoke("ResetSpeedMultiplier", powerUpDuration);
    }

    private void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }
}
