using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevelName;
    public string levelSelect;
    public string[] levelNames;
    public int startingLives = 3;
    void Start() {
        
    }

    void Update() {
        
    }
    public void NewGame(){
        for (int i = 0; i < levelNames.Length; i++) {
            PlayerPrefs.SetInt( levelNames[i] , 0 );
        }
        PlayerPrefs.SetInt("CoinCount",0);
        PlayerPrefs.SetInt("PlayerLives",startingLives);
        SceneManager.LoadScene( firstLevelName );
    }
    public void Continue(){
        SceneManager.LoadScene( levelSelect );
    }
    public void Quit(){
        Application.Quit();
    }
}
