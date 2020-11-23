using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle,Playing,Ended,Ready};

public class Game_Controller : MonoBehaviour
{
    [Range(0f,0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public Text pointsText;
    

    public GameState gameState = GameState.Idle;
    public GameObject player;
    public GameObject enemyGenerator;
    private AudioSource musicPlayer;

    public float scaleTime = 6f;//cada cuanto se incrementa.
    public float scaleInc = 0.25f;//cantidad de incrementacion.
    private int points = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool userAction = (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));
        
        //Empieza el juego
        if ( (gameState== GameState.Idle) && userAction)
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "PlayerRun");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }

        //Juego en marcha
        else if(gameState ==GameState.Playing)
        {
            Parallax();
        }

        //Condicion para reiniciar el juego.
        else if (gameState == GameState.Ready)
        {
            if (userAction){
                RestartGame();
            }
        }

    }

    public void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incrementado: " + Time.timeScale);
    }

    public void ResetTimeScale()
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
        Debug.Log("Ritmo restablecido");
    }

    public void IncreasePoint()
    {
        pointsText.text ="Score: "+(++ points).ToString();
    }
}
