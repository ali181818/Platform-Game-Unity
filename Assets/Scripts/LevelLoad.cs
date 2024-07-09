using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour {
    public string levelToLoad;
    public bool unlocked;
    public Sprite doorOpen , doorClosed;
    public SpriteRenderer door;
    void Start() {

        if( PlayerPrefs.GetInt( "Reset" ) != 1 )
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Reset",1);

        PlayerPrefs.SetInt("Level 1",1);

        if( PlayerPrefs.GetInt( levelToLoad ) == 1 ) {
            unlocked = true;
        } else {
            unlocked = false;
        }

        if( unlocked ) {
            door.sprite = doorOpen;
        } else {
            door.sprite = doorClosed;
        }
    }
    void OnTriggerStay2D( Collider2D other ) {
        if( other.CompareTag("Player") ) {
            // if( Input.GetButtonDown("Jump") )
            if( ( Input.GetKey(KeyCode.F) || Input.GetButton("Jump") ) && unlocked )
                SceneManager.LoadScene( levelToLoad );
        }
    }
}
