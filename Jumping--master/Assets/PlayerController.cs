using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;

    private Animator animator;
    private AudioSource audioPlayer;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    private float startY;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = transform.position.y == startY;
        bool userAction = (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));
        bool gamePlaying = game.GetComponent<Game_Controller>().gameState == GameState.Playing;

        if (isGrounded && gamePlaying && userAction)
        {
            UpdateState("PlayerJump");

            //Musica del juego salto
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("Playerdie");
            game.GetComponent<Game_Controller>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale");

            //Musica del juego muerte
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        }
        else if (other.gameObject.tag == "point")
        {
            game.SendMessage("IncreasePoint");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }

        
    }

    void GameReady()
    {
        game.GetComponent<Game_Controller>().gameState = GameState.Ready;
    }
}
