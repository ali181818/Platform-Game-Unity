using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHeadEnemy : MonoBehaviour
{
    Rigidbody2D playerRigidbody;
    public float bounceSpeed = 5f;
    void Start() {
        playerRigidbody = transform.parent.GetComponent<Rigidbody2D>();
    }
    void Update() {
        
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Enemy") ){
            // Destroy( other.gameObject );
            other.gameObject.SetActive(false);
            playerRigidbody.velocity = new Vector3( playerRigidbody.velocity.x , bounceSpeed , 0 );
        }
        if( other.CompareTag("Boss") ){
            playerRigidbody.velocity = new Vector3( playerRigidbody.velocity.x , bounceSpeed , 0 );
            other.transform.parent.GetComponent<Boss>().takeDamage = true;
        }
    }
}
