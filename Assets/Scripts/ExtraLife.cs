using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    public int liveToGive = 1;

    LevelManager theLevelManager;
    void Start() {
        theLevelManager = FindObjectOfType<LevelManager>(); 
    }
    void Update() {
        
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Player")) {
            theLevelManager.AddLives( liveToGive );
            gameObject.SetActive( false ); 
        }
    }
}
