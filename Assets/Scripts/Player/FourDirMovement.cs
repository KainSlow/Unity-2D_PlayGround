using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDirMovement : MonoBehaviour
{
    [SerializeField]
    KeyCode up, down, left, right;
    [SerializeField]
    float speed;

    Vector2 movDir = Vector2.zero;

    KeyCode currentKey;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(up))  currentKey = up;
        else if (Input.GetKeyDown(down))  currentKey = down; 
        else if (Input.GetKeyDown(left))  currentKey = left; 
        else if (Input.GetKeyDown(right))  currentKey = right;

        if (Input.GetKeyUp(currentKey))
        {
            if (Input.GetKey(up)) currentKey = up;
            else if (Input.GetKey(down)) currentKey = down;
            else if (Input.GetKey(left)) currentKey = left;
            else if (Input.GetKey(right)) currentKey = right;
            else
            {
                currentKey = KeyCode.None;
            }
        }

        movDir = Vector2.zero;
        if(currentKey == up)
        {
            movDir = new(0f, movDir.y + 1f);
        }

        if(currentKey == down)
        {
            movDir = new(0f, movDir.y + -1f);

        }

        if(currentKey == left)
        {
            movDir = new(movDir.x + -1f, 0f);
        }

        if(currentKey == right)
        {
            movDir = new(movDir.x + 1f, 0f);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = speed * movDir.normalized;
    }

}
