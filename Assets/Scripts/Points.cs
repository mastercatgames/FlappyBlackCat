using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{

    public GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void OnTriggerEnter2D(Collider2D colisor)
    {
        gameController.Score++;
        gameController.scoreText.text = gameController.Score.ToString();
        // gameController.point_sfx.Play();
    }
}
