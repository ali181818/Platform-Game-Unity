using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    LevelManager theLevelManger;
    public int damageToGive = 1;

    void Start() {
        theLevelManger = FindAnyObjectByType<LevelManager>();
    }

    void Update() {
        
    }

    void OnTriggerEnter2D( Collider2D obj ) {
        if( obj.CompareTag("Player")) {
            // theLevelManger.Respawn();
            theLevelManger.HurtPlayer( damageToGive );
        }
    }
}
