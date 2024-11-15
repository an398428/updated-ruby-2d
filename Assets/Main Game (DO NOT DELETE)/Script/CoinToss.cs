using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    //D.W. ADDED for Coin Throw Distance
    public float changeTime = 3.0f;


    // Awake is called when the Projectile GameObject is instantiated
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }



public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);

        //D.W. ADDED for Coint Throw Distance
        changeTime = 3.0f;
        if (changeTime < 1)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.collider.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
        }


        Destroy(gameObject);
    }

}