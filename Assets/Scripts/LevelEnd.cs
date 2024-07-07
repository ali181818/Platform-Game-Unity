using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string levelName;
    Player thePlayer;
    CameraController theCameraController;
    public float waitToMove = 1f;
    public float waitToLoad = 2f;
    public Sprite flagOpen;
    SpriteRenderer theSprite;

    LevelManager theLevelManager;

    public string levelToUnlock;

    bool movePlayer;
    void Start() {
        thePlayer = FindObjectOfType<Player>();
        theCameraController = FindAnyObjectByType<CameraController>();
        theLevelManager = FindAnyObjectByType<LevelManager>();
        theSprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if( movePlayer ) {
            thePlayer.transform.localScale = new Vector3( 2f , 2f , 0 );
            thePlayer.anim.SetFloat("speed",1);
            thePlayer.myRigidBody.velocity = new Vector3( thePlayer.speed , thePlayer.myRigidBody.velocity.y , 0);
        }
    }
    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("Player") ) {
            theSprite.sprite = flagOpen;
            StartCoroutine(DoLevelEnd());
        }
    }

    IEnumerator DoLevelEnd() {
        thePlayer.anim.SetFloat("speed",-1);
        thePlayer.anim.SetBool("GroundJump",true);
        thePlayer.canMove = false;
        theCameraController.followTarget = false;
        thePlayer.myRigidBody.velocity = Vector3.zero;

        PlayerPrefs.SetInt("CoinCount" , theLevelManager.coinCount );
        PlayerPrefs.SetInt("PlayerLives" , theLevelManager.currentLive );

        PlayerPrefs.SetInt( levelToUnlock , 1 );

        yield return new WaitForSeconds(waitToMove);
        movePlayer = true;
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene( levelName );
    }
}
