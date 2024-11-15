using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnakeControllerOne : SnakeController
{

    // Update is called once per frame
    protected override void Update()
    {
        // Check for player input and update the direction
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
        {
            nextDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
        {
            nextDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
        {
            nextDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
        {
            nextDirection = Vector2.right;
        }
    }

}
