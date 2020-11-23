using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rig;
    private AudioSource jump_sfx;
    public GameObject GameOver;
    private GameController gameController;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        jump_sfx = GetComponent<AudioSource>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (!GameOver.activeSelf
        && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            rig.velocity = Vector2.up * speed;
            jump_sfx.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D colisor)
    {
         GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 0.2f;
        //gameController.StartCoroutine(gameController.FadeSound());
        GameOver.SetActive(true);
        Time.timeScale = 0;
    }
}
