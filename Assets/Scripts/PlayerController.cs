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
    public Transform targetToMovePlayer;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //jump_sfx = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        //targetToMovePlayer = new Vector3(0, transform.position.y, transform.position.z);
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
            // transform.position = Vector3.MoveTowards(transform.position, targetToMovePlayer.position, 0.5f * Time.deltaTime);

            // //Check if the position of the cube and sphere are approximately equal. (When done..)
            // if (Vector3.Distance(transform.position, targetToMovePlayer.position) < 0.001f)
            // { 
            //     showingPlayer = false;
            // }
            //transform.position += Vector3.right * Time.deltaTime;
            //transform.position += new Vector3(3f * Time.deltaTime , 0, 0);

            

            transform.position = Vector3.Lerp(transform.position, targetToMovePlayer.position, 0.8f * Time.deltaTime);

            //Check if the position of the cube and sphere are approximately equal. (When done..)
            if (Vector3.Distance(transform.position, targetToMovePlayer.position) < 0.001f)
            { 
                showingPlayer = false;
            }

            //transform.position = Vector3.Lerp(transform.position, targetToMovePlayer, Time.deltaTime);

        //     Vector3 newPos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        //  transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 1f);
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
