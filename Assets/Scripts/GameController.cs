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
    public Text gameplayText;
    public GameObject menu;
    public GameObject PlayButton;
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
        
        Time.timeScale = 1;

        point_sfx = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        ShowMenu();
    }

    public void StartGame()
    {
        //menu.SetActive(false);        
        //menu.GetComponent<Animator>().Play("FadeOut_up_to_down");
        HideMenu();
        firstTapButton.SetActive(true);
        gameplayText.gameObject.SetActive(true);        
        gameplayText.GetComponent<Animator>().Play("FadeIn_down_to_up");

        playerController.showingPlayer = true;
    }

    private void ShowMenu()
    {
        foreach (Transform buttons in menu.transform)
        {
            buttons.GetComponent<Animator>().Play("FadeIn");
        }
    }

    private void HideMenu()
    {
        menu.GetComponent<Animator>().Play("FadeOut_up_to_down");

        foreach (Transform buttons in menu.transform)
        {
            if (buttons.GetComponent<Button>() != null)
                buttons.GetComponent<Button>().interactable = false;

            buttons.GetComponent<Animator>().Play("FadeOut");
            buttons.GetComponent<Animator>().speed = 3f;
        }
    }

    private void FadeOutTextAfterTime()
    {
        gameplayText.GetComponent<Animator>().Play("FadeOut");
    }
    
    public void FirstTap()
    {
        firstTapButton.SetActive(false);
        playerController.GetComponent<Rigidbody2D>().simulated = true;
        playerController.Jump();

        jumpButton.SetActive(true);
        spawnVacuums.SetActive(true);        
        scoreText.gameObject.SetActive(true);        

        scoreText.GetComponent<Animator>().Play("FadeIn");

        if (gameplayText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn_down_to_up"))
        {
            //If animation to show text still running, run FadeOut after time
            print("If animation to show text still running, run FadeOut after time");
            Invoke("FadeOutTextAfterTime", 1.5f);
            gameplayText.GetComponent<Animator>().speed = 1.5f;
        }
        else
        {
            gameplayText.GetComponent<Animator>().Play("FadeOut");
        }
    }

    public void GameOver()
    {
        StartCoroutine(FadeAudioSource.StartFade(music, 3f, 0f));
        gameOverPanel.SetActive(true);
        jumpButton.SetActive(false);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 1f;
    }

}
