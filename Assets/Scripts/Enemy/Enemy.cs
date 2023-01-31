using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    bool walkingLeft;
    bool walkingRight;
    float localX;

    void Start()
    {
        health = maxHealth;
        walkingLeft = false;
        walkingRight = true;
    }

    void Update()
    {
        Walk();

        if(localX < 0)
        {
            walkingRight = true;
            walkingLeft = false;
            transform.localScale = new Vector3(0.7f, 0.84f, 0.7f);
        }
        else if(localX > 3)
        {
            walkingRight = false;
            walkingLeft = true;
            transform.localScale = new Vector3(-0.7f, 0.84f, 0.7f);
        }
        
    }

    void Walk()
    {
        if (walkingRight)
        {
            gameObject.transform.Translate(0.025f, 0, 0);
            localX = localX + 0.025f;
        }
        else if (walkingLeft)
        {
            gameObject.transform.Translate(-0.025f, 0, 0);
            localX = localX - 0.025f;
        }
    }
}
