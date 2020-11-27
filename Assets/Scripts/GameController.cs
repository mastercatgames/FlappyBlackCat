﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ************** Flappy Black Cat ********************
public class GameController : MonoBehaviour
{
    private PlayerController playerController;
    [Header("=== UI ===")]
    public int Score;
    public Text scoreText;
    public GameObject menu;
    public GameObject firstTapButton;
    public GameObject jumpButton;
    public GameObject spawnVacuums;
    public GameObject gameOverPanel;    
    [Header("=== Audio ===")]
    public AudioSource point_sfx;
    public AudioSource music;
    public float fadeTime = 1;
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        point_sfx = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        menu.SetActive(false);
        firstTapButton.SetActive(true);
    }
    
    public void FirstTap()
    {
        firstTapButton.SetActive(false);
        playerController.GetComponent<Rigidbody2D>().simulated = true;
        playerController.Jump();

        jumpButton.SetActive(true);

        spawnVacuums.SetActive(true);        

    }

    public void GameOver()
    {
        StartCoroutine(FadeAudioSource.StartFade(music, 3f, 0f));
        gameOverPanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 1f;
    }

}
