using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    LevelManager theLevelManager;
    public int CoinValue = 5;
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
    }
    void Update()
    {
        
    }
    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Player")) {
            theLevelManager.AddCoin( CoinValue );
            // Destroy( gameObject );
            gameObject.SetActive(false);
        }
    }
}
