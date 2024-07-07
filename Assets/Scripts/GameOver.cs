using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string levelSelect , mainMenu;

    LevelManager theLevelManager;
    public GameObject gameOverScreen;
    void Start() {
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update() {
        
    }
    public void Restart(){
        PlayerPrefs.SetInt("CoinCount",0);
        PlayerPrefs.SetInt("PlayerLives",theLevelManager.startingLive);
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
    public void LevelSelected(){
        PlayerPrefs.SetInt("CoinCount",0);
        PlayerPrefs.SetInt("PlayerLives",theLevelManager.startingLive);
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene( levelSelect );
    }
    public void MainMenuSelect(){
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene( mainMenu );
    }
}
