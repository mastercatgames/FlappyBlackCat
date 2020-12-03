using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rig;
    private AudioSource jump_sfx;
    private GameController gameController;
    private AudioSource music;
    public bool showingPlayer;
    public bool resetPlayerPosition;
    public Transform targetToMovePlayer;
    public Transform originPlayerPosition;
    public float speedToShowPlayer;    

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //jump_sfx = GetComponent<AudioSource>();
        //gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //gameController = GameObject.FindGameObjectWithTag("UI").GetComponent<GameController>();
        gameController = FindObjectOfType<GameController>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        speedToShowPlayer = 0.7f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (showingPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, targetToMovePlayer.position, speedToShowPlayer * Time.deltaTime);

            //Check if the position of the cube and sphere are approximately equal. (When done..)
            if (Vector3.Distance(transform.position, targetToMovePlayer.position) < 0.1f)
            { 
                showingPlayer = false;
                gameController.ShowFirstTapButton();
            }

            if (Vector3.Distance(transform.position, targetToMovePlayer.position) < 0.2f)
            {
                //Hide ReadyText before finish the showingPlayer transition
                gameController.ReadyText.GetComponent<Animator>().Play("FadeOut");
            }
        }

        if (resetPlayerPosition)
        {
            transform.position = Vector3.Lerp(transform.position, originPlayerPosition.position, speedToShowPlayer * Time.deltaTime);

            //Check if the position of the cube and sphere are approximately equal. (When done..)
            if (Vector3.Distance(transform.position, originPlayerPosition.position) < 0.1f)
            { 
                resetPlayerPosition = false;
                gameController.ShowFirstTapButton();
            }

            if (Vector3.Distance(transform.position, targetToMovePlayer.position) < 0.2f)
            {
                //Hide ReadyText before finish the showingPlayer transition
                gameController.ReadyText.GetComponent<Animator>().Play("FadeOut");
            }
        }
    }

    public void Jump()
    {
        rig.velocity = Vector2.up * speed;
    }
    void OnCollisionEnter2D(Collision2D colisor)
    {
        //  GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 0.2f;
        gameController.GameOver();

    }
}
