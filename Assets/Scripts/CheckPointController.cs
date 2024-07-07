using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Sprite flagRed , flagGreen;
    SpriteRenderer spriteFlag;
    void Start()
    {
        spriteFlag = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D( Collider2D player ) {
        if( player.tag == "Player" ) {
            spriteFlag.sprite = flagGreen;
        }
    }
}
