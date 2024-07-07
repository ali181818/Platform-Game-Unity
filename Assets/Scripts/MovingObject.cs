using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform startPoint , endPoint;
    public float moveSpeed = 2f;
    Vector3 currentTarget;
    public Vector3 currentPosition ;
    void Start() {
        currentTarget = endPoint.position;
    }

    void Update() {
        currentPosition = objectToMove.transform.position;
        objectToMove.transform.position = Vector3.MoveTowards( objectToMove.transform.position , currentTarget , moveSpeed * Time.deltaTime ); 

        if( objectToMove.transform.position == endPoint.position ) {
            currentTarget = startPoint.position;
        }
        if( objectToMove.transform.position == startPoint.position ) {
            currentTarget = endPoint.position;
        }
    }
}
