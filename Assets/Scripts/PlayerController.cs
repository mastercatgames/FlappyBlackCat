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

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        //jump_sfx = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
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
