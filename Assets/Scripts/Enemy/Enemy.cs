using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    bool walkingLeft;
    bool walkingRight;

    void Start()
    {
        health = maxHealth;
        walkingLeft = false;
        walkingRight = true;
    }

    void Update()
    {
        Walk();
        //Shoot a raycast a distance of 1 to the left and right and check if it hits a wall.
        //If it does hit a wall, turn around.
        RaycastHit2D wallDetectionRight = Physics2D.Raycast(transform.position, Vector2.up, 1.5f);
        RaycastHit2D wallDetectionLeft = Physics2D.Raycast(transform.position, -Vector2.right, 1.5f);

        Debug.Log("Right detection: " + wallDetectionRight.collider.tag);
        Debug.Log("Left detection: " + wallDetectionLeft.collider.tag);

        if(wallDetectionRight.collider.tag == "Wall")
        {
            walkingRight = false;
            walkingLeft = true;
        }
        if (wallDetectionLeft.collider.tag == "Wall")
        {
            walkingRight = true;
            walkingLeft = false;
        }

        Debug.DrawLine(transform.position, transform.position + new Vector3(-1.5f, 0f, 0f));
        Debug.DrawLine(transform.position, transform.position + new Vector3(1.5f, 0f, 0f));
    }

    void Walk()
    {
        if (walkingRight)
        {
            gameObject.transform.Translate(0.1f, 0, 0);
        }
        else if (walkingLeft)
        {
            gameObject.transform.Translate(-0.1f, 0, 0);
        }
    }
}
