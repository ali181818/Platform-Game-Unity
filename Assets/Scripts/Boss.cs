using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool bossActive;
    public float timeBetweenDrops = 2f;
    float dropCount;
    public float waitForPlatform = 5f;
    float platformCount;

    public Transform leftPoint , rightPoint , dropSawSpawnPoint;
    public GameObject dropSaw;

    public GameObject theBoss;
    public bool bossRight;
    public GameObject rightPlatform , leftPlatform;
    public bool takeDamage;
    public int startingHealth = 3;
    public int currentHealth;
    CameraController theCam;

    void Start() {
        dropCount = timeBetweenDrops;
        platformCount = waitForPlatform;

        theBoss.transform.position = rightPoint.position;
        bossRight = true;

        currentHealth = startingHealth;

        theCam = FindAnyObjectByType<CameraController>();

    }
    void Update() {
        if( bossActive ) {
            theCam.followTarget = false;
            theBoss.SetActive( true );
            if( dropCount > 0 ) {
                dropCount -= Time.deltaTime;
            } else {
                dropSawSpawnPoint.position = new Vector3( Random.Range(leftPoint.position.x , rightPoint.position.x),dropSawSpawnPoint.position.y,dropSawSpawnPoint.position.z);
                Instantiate( dropSaw , dropSawSpawnPoint.position , dropSawSpawnPoint.rotation );
                dropCount = timeBetweenDrops;

            }

            if( bossRight ) {
                if( platformCount > 0 ) {
                    platformCount -= Time.deltaTime;
                } else {
                    rightPlatform.SetActive( true );
                }
            } else {
                if( platformCount > 0 ) {
                    platformCount -= Time.deltaTime;
                } else {
                    leftPlatform.SetActive( true );
                }
            }
        }
        if( takeDamage ) {
            currentHealth -= 1;
            if( currentHealth <= 0 ) {
                theCam.followTarget = true;
                gameObject.SetActive( false ); 
            }
            if( bossRight ) {
                theBoss.transform.position = leftPoint.position;
            } else {
                theBoss.transform.position = rightPoint.position;
            }
            bossRight = !bossRight;
            rightPlatform.SetActive( false );
            leftPlatform.SetActive( false );
            platformCount = waitForPlatform;
            timeBetweenDrops = timeBetweenDrops / 2 ;
            takeDamage = false;
        }
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Player") ) {
            bossActive = true;
        }
    }
}
