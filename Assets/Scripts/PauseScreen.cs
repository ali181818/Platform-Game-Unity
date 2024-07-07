using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public string levelSelect , mainMenu;

    LevelManager theLevelManager;
    public GameObject PauseMenuScreen;
    void Start() {
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update() {
        if( Input.GetKeyDown( KeyCode.Escape )){
            if( Time.timeScale == 0 ) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }
    public void PauseGame() {
        theLevelManager.levelMusic.Pause();
        PauseMenuScreen.SetActive( true );
        Time.timeScale = 0;
    }
    public void ResumeGame(){
        Time.timeScale = 1;
        theLevelManager.levelMusic.Play();
        PauseMenuScreen.SetActive(false);
    }
    public void LevelSelected(){
        PlayerPrefs.SetInt("CoinCount",theLevelManager.coinCount );
        PlayerPrefs.SetInt("PlayerLives",theLevelManager.startingLive);
        PauseMenuScreen.SetActive(false);
        SceneManager.LoadScene( levelSelect );
    }
    public void MainMenuSelect(){
        PauseMenuScreen.SetActive(false);
        SceneManager.LoadScene( mainMenu );
    }
}
