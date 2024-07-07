using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnRespawn : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 startLocalScale;
    Rigidbody2D myRigidbody;
    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startLocalScale = transform.localScale;

        if( GetComponent<Rigidbody2D>() != null ) {
            myRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    void Update() {
        
    }

    public void ResetObject() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startLocalScale;

        if( myRigidbody != null ) {
            myRigidbody.velocity = Vector3.zero;
        }
    }
}
