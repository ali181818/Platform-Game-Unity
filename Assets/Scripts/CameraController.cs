using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float forwardPlayer = 2f;

    Vector3 targetPosition;
    public float smoothing = 2f; 
    public bool followTarget = true;

    void Start()
    {
        
    }

    void Update()
    {
        targetPosition = new Vector3( target.transform.position.x , transform.position.y , transform.position.z );

        if( followTarget ) {
            if( target.transform.localScale.x > 0f ) {
                targetPosition = new Vector3( target.transform.position.x + forwardPlayer , transform.position.y , transform.position.z );
            } else {
                targetPosition = new Vector3( target.transform.position.x - forwardPlayer , transform.position.y , transform.position.z );
            }

            transform.position = Vector3.Lerp( transform.position , targetPosition , smoothing * Time.deltaTime );
        }
    }
}
