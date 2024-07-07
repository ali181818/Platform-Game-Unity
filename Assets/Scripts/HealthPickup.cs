using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthToGive = 2;
    LevelManager theLevelManager;
    void Start() {
        theLevelManager = FindAnyObjectByType<LevelManager>();
    }

    void Update() {
        
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Player") )  {
            theLevelManager.GiveHealth( healthToGive );
            gameObject.SetActive(false);
        }
    }
}
