using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeTime = 1f;
    Image blackScreen;
    void Start() {
        blackScreen = GetComponent<Image>();
    }
    void Update() {
        blackScreen.CrossFadeAlpha( 0f , fadeTime , false );

        if(blackScreen.color.a == 0){
            gameObject.SetActive( false );
        }
        // blackScreen.tintColor = new Color( r: 0 , g: 0 , b: 0 , a: .5f );
    }
}
