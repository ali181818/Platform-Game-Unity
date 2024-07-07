using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform pointRight , pointLeft;
    public float moveSpeed = 3f;
    Rigidbody2D myRigidbody;
    public bool movingRight;
    void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if( !movingRight && transform.position.x < pointLeft.position.x ) {
            movingRight = true;
            transform.localScale = new Vector3( 0.9f , 0.9f , 0 );
        } 
        if( movingRight && transform.position.x > pointRight.position.x ) {
            movingRight = false;
            transform.localScale = new Vector3( -0.9f , 0.9f , 0 );
        } 
        if( movingRight ) {
            myRigidbody.velocity = new Vector3( moveSpeed , myRigidbody.velocity.y , 0f);
        }
        else {
            myRigidbody.velocity = new Vector3( - moveSpeed , myRigidbody.velocity.y , 0f);
        }
    }
}
