using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{

    public float moveSpeed = 3f;
    bool canMove;
    Rigidbody2D myRigidBody;
    void Start(){
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if( canMove ) {
            myRigidBody.velocity = new Vector3 ( -moveSpeed , myRigidBody.velocity.y , 0 );
        }
    }

    void OnBecameVisible(){
        canMove = true;
    }

    void OnTriggerEnter2D( Collider2D other ){
        if( other.CompareTag("KillZone") ) {
            // Destroy( gameObject );
            gameObject.SetActive(false);
        }
    }

    void OnEnable(){
        canMove = false;
    }
}
