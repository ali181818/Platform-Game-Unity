using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public float jumpSpeed = 10;

    public Rigidbody2D myRigidBody;

    public bool facingRight;

    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask isGround;
    public bool isGroundedCheck;
    public Vector3 respownPosition;
    public LevelManager theLevelManager;
    public GameObject killBox;
    public float knockForce = 5f;
    public float knockLength = 0.25f;
    float knockCounter;

    public AudioSource jumpSound , hitSound;

    public float onPlatformSpeed = .5f;
    bool onPlatform;
    float activeMoveSpeed;

    public bool canMove = true;

    public Animator anim;

    public GameObject ammo;
    public Transform fireMarker;
    void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respownPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();

        activeMoveSpeed = speed;
    }

    void Update() {

        isGroundedCheck = Physics2D.OverlapCircle( groundCheck.position , groundCheckRadius , isGround );

        float move = Input.GetAxis("Horizontal");


        if( knockCounter <= 0 && canMove ) {
            
            anim.SetFloat("speed", Mathf.Abs(move) );

            if( onPlatform ) {
                activeMoveSpeed = onPlatformSpeed * speed ;
            } else {
                activeMoveSpeed = speed;
            }
            myRigidBody.velocity = new Vector2( move * activeMoveSpeed , myRigidBody.velocity.y );

            if( move > 0 && facingRight )
                Direction();
            else if( move < 0 && !facingRight )
                Direction();

            if( Input.GetButtonDown("Jump") && isGroundedCheck ) {
                jumpSound.Play();
                myRigidBody.velocity = new Vector2( myRigidBody.velocity.x , jumpSpeed);
            }
        }
        if( knockCounter > 0 ) {
            knockCounter -= Time.deltaTime;
            if( transform.localScale.x > 0 ) {
                myRigidBody.velocity = new Vector3( -knockForce , knockForce , 0);
            } else {
                myRigidBody.velocity = new Vector3( knockForce , knockForce , 0);
            }

        }



        if( myRigidBody.velocity.y < 0 ) {
            killBox.SetActive(true);
        } else {
            killBox.SetActive(false);
        }


        anim.SetBool("GroundJump", isGroundedCheck );

        if( Input.GetKey( KeyCode.G ) || Input.GetButton( "Fire1" ) ) {
            anim.SetInteger( "shoot" , 2 );
            myRigidBody.velocity = Vector2.zero;
        } else if ( myRigidBody.velocity.x != 0 ) {
            anim.SetInteger( "shoot" , 3 );
        } else {
            anim.SetInteger( "shoot" , 1 );
        }

    }

    void Direction(){
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;

        scale.x *= -1;

        transform.localScale = scale;
    }

    void OnTriggerEnter2D( Collider2D other ) {
        if( other.CompareTag("KillZone") ) {
            theLevelManager.Respawn();
        }
        if( other.CompareTag("CheckPoint") ) {
            respownPosition = other.transform.position;
        }
    }

    void OnCollisionEnter2D( Collision2D other ) {
        if( other.gameObject.CompareTag("MovingPlatform")){
            onPlatform = true;
            transform.parent = other.transform;
        }
    }
    void OnCollisionExit2D( Collision2D other ) {
        if( other.gameObject.CompareTag("MovingPlatform")){
            onPlatform = false;
            transform.parent = null;
        }
    }

    public void knockBack(){
        knockCounter = knockLength;
    }
    public void Fire() {
        if( ammo != null ) {
            var clone = Instantiate( ammo , fireMarker.position , quaternion.identity ) as GameObject;
            clone.transform.localScale = transform.localScale;
        }
    }
}
