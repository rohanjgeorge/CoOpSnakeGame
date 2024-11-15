using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakeControllerTwo : SnakeController
{
    
    // Update is called once per frame
    protected override void Update()
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

}
