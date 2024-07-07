using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 myVelocity = new Vector2( 5 , 0 );
    Rigidbody2D myRigidBody;
    public float distance = 5f;
    void Start()
    {
        distance += Mathf.Abs( transform.position.x );
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.velocity = myVelocity * transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if( Math.Abs( transform.position.x ) > distance ) {
            Destroy( gameObject );
        }
    }
}
