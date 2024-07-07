using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float waitToRespawn = 2f;
    public Player thePlayer;
    public int coinCount;
    public Text coinText;
    public Image heart1 , heart2 , heart3;
    public Sprite heartFull , heartHalf , heartEmpty;

    public int maxHealth = 6;
    public int healthCount;

    public ResetOnRespawn[] ObjectToRespawn;
    public int currentLive;
    public int startingLive = 3;
    public Text liveText;

    public GameObject gameOverScreen;

    public int coinBonusLifeCount;
    public int ValueBonus = 15;
    public AudioSource pickUpSound;
    public AudioSource levelMusic , GameOverMusic;

    void Start() {
        currentLive = startingLive;
        if( PlayerPrefs.HasKey("CoinCount"))
            coinCount = PlayerPrefs.GetInt("CoinCount");
        if( PlayerPrefs.HasKey("PlayerLives"))
            currentLive = PlayerPrefs.GetInt("PlayerLives");

        thePlayer = FindObjectOfType<Player>();
        coinText.text = "Coins: " + coinCount;

        healthCount = maxHealth;

        ObjectToRespawn = FindObjectsOfType<ResetOnRespawn>();

        liveText.text = "Lives X " + currentLive;
    }

    void Update() {
        
    }
    public void Respawn() {
        currentLive -= 1;
        liveText.text = "Lives X " + currentLive;
        if( currentLive > 0 ) {
            StartCoroutine("RespawnDelay");
        } else {
            thePlayer.gameObject.SetActive( false );
            gameOverScreen.SetActive( true );
            levelMusic.Stop();
            GameOverMusic.Play();
        }
    }
    IEnumerator RespawnDelay() {
        thePlayer.gameObject.SetActive( false );

        healthCount = maxHealth;
        yield return new WaitForSeconds(waitToRespawn);
        UpdateHeart();
        
        thePlayer.transform.position = thePlayer.respownPosition;
        thePlayer.gameObject.SetActive( true );

        coinCount = 0;
        coinBonusLifeCount = 0;
        coinText.text = "Coins: " + coinCount;

        foreach (ResetOnRespawn item in ObjectToRespawn) {
            item.ResetObject();
            item.gameObject.SetActive( true );
        }

    }

    public void AddCoin( int coinToAdd ) {
        coinCount += coinToAdd;
        coinBonusLifeCount += coinToAdd;
        if( coinBonusLifeCount >= ValueBonus ) {
            currentLive += 1;
            liveText.text = "Lives X " + currentLive;
            coinBonusLifeCount = 0;
        }
        coinText.text = "Coins: " + coinCount;
        pickUpSound.Play();
    }

    public void HurtPlayer( int damageToTake ) {
        healthCount -= damageToTake;
        UpdateHeart();
        thePlayer.knockBack();
        if( healthCount <= 0 ) {
            Respawn();
        }
        thePlayer.hitSound.Play();
    }

    public void UpdateHeart() {
        switch ( healthCount ) {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                break;
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                break;
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                break;
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                break;
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;
            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                break;
        }
    }

    public void  AddLives( int livesToAdd ) {
        currentLive += livesToAdd;
        liveText.text = "Lives X " + currentLive;
        pickUpSound.Play();
    }
    public void GiveHealth( int healthToGive ){
        healthCount += healthToGive;
        if( healthCount > maxHealth ) {
            healthCount = maxHealth;
        }
        UpdateHeart();
        pickUpSound.Play();
    }
}
