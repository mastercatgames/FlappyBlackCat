﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ************** Flappy Black Cat ********************
public class GameController : MonoBehaviour
{
    private PlayerController playerController;
    private SpawnPipes spawnPipes;
    private UnityAds unityAds;

    [Header("♦ UI")]
    public int Score;
    public Text scoreText;
    public Text TapToFlyText;
    public Text ReadyText;
    public GameObject menu;
    public GameObject PlayButton;
    public GameObject firstTapButton;
    public GameObject jumpButton;
    public GameObject gameOverPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;

    [Header("♦ Audio")]
    public AudioSource music;
    // public AudioSource point_sfx;    
    public float fadeTime = 1;

    [Header("♦ Other variables")]
    public List<string> readyRamdomTexts;
    public string difficulty;
    public bool isRetry;
    public bool isContinue;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnPipes = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("SpawnVacuums").GetComponent<SpawnPipes>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        unityAds = GetComponent<UnityAds>();

        Time.timeScale = 1;

        //Force to increase volume if player click in "Home" button
        if (music.volume < 1)
        {
            StartCoroutine(FadeAudioSource.StartFade(music, 1f, 1f));
        }

        ShowMenu();
        LoadReadyRamdomTexts();

        if (PlayerPrefs.GetString("difficulty") == "")
        {
            PlayerPrefs.SetString("difficulty", "Easy");
        }

        difficulty = PlayerPrefs.GetString("difficulty");
    }

    private void LoadReadyRamdomTexts()
    {
        readyRamdomTexts = new List<string>();
        readyRamdomTexts.Add("Enjoy");
        readyRamdomTexts.Add("Relax");
        readyRamdomTexts.Add("Thanks for playing!");
        readyRamdomTexts.Add("Change the difficulty in options anytime");
        readyRamdomTexts.Add("Use headphones for better experience");
    }

    public void StartGame()
    {
        HideMenu();

        ReadyText.gameObject.SetActive(true);
        ReadyText.text = readyRamdomTexts[Random.Range(0, readyRamdomTexts.Count)];
        ReadyText.GetComponent<Animator>().Play("FadeIn_down_to_up");

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
        menu.GetComponent<Animator>().Play("FadeOut");

        foreach (Transform buttons in menu.transform)
        {
            if (buttons.GetComponent<Button>() != null)
                buttons.GetComponent<Button>().interactable = false;

            buttons.GetComponent<Animator>().Play("FadeOut");
            buttons.GetComponent<Animator>().speed = 3f;
        }
    }

    private void HideGameOver()
    {
        foreach (Transform buttons in gameOverPanel.transform)
        {
            if (buttons.GetComponent<Button>() != null)
                buttons.GetComponent<Button>().interactable = false;

            if (buttons.GetComponent<Animator>() == null)
            {
                buttons.Find("Text").GetComponent<Animator>().Play("FadeOut");
                buttons.Find("Text").GetComponent<Animator>().speed = 3f;
            }
            else
            {
                buttons.GetComponent<Animator>().Play("FadeOut");
                buttons.GetComponent<Animator>().speed = 3f;
            }

            if (buttons.gameObject.name == "ContinueButton")
            {
                buttons.Find("Icon").GetComponent<Animator>().Play("FadeOut");
                buttons.Find("Icon").GetComponent<Animator>().speed = 3f;
                buttons.Find("Text").GetComponent<Animator>().Play("FadeOut");
                buttons.Find("Text").GetComponent<Animator>().speed = 3f;                
            }
            else if (buttons.gameObject.name == "NoAds")
            {
                buttons.Find("Price").GetComponent<Animator>().Play("FadeOut");
                buttons.Find("Price").GetComponent<Animator>().speed = 3f;             
            }

            Invoke("AfterHideGameOver", 1f);
        }
    }

    private void AfterHideGameOver()
    {
        gameOverPanel.SetActive(false);
        foreach (Transform buttons in gameOverPanel.transform)
        {
            buttons.gameObject.SetActive(false);

            if (buttons.GetComponent<Button>() != null)
                buttons.GetComponent<Button>().interactable = true;
        }

        if (isRetry)
        {
            gameOverPanel.transform.Find("ScoreNum").Find("Text").GetComponent<Text>().text = "0";
            scoreText.text = "0";
            isRetry = false;
        }
    }

    public void ShowFirstTapButton()
    {
        firstTapButton.SetActive(true);
        TapToFlyText.text = "Tap to fly";
        TapToFlyText.gameObject.SetActive(true);
        TapToFlyText.GetComponent<Animator>().Play("FadeIn");
    }

    private void FadeOutTextAfterTime()
    {
        TapToFlyText.GetComponent<Animator>().Play("FadeOut");
    }

    public void FirstTap()
    {
        firstTapButton.SetActive(false);
        playerController.GetComponent<Rigidbody2D>().simulated = true;
        playerController.Jump();

        jumpButton.SetActive(true);
        // spawnVacuums.SetActive(true);
        scoreText.gameObject.SetActive(true);

        scoreText.GetComponent<Animator>().Play("FadeIn");

        if (TapToFlyText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn_down_to_up"))
        {
            //If animation to show text still running, run FadeOut after time
            print("If animation to show text still running, run FadeOut after time");
            Invoke("FadeOutTextAfterTime", 1.5f);
            TapToFlyText.GetComponent<Animator>().speed = 1.5f;
        }
        else
        {
            TapToFlyText.GetComponent<Animator>().Play("FadeOut");
        }

        spawnPipes.InvokeRepeating("SpawnVacuum_" + difficulty, 0f, spawnPipes.repeatRate);
    }

    public void GameOver()
    {
        StartCoroutine(FadeAudioSource.StartFade(music, 1f, 0.4f));

        gameOverPanel.SetActive(true);
        jumpButton.SetActive(false);
        scoreText.GetComponent<Animator>().Play("FadeOut");
        scoreText.GetComponent<Animator>().speed = 3f;
        spawnPipes.CancelInvoke();

        if (PlayerPrefs.GetInt("removeAds") == 1)
        {
            gameOverPanel.transform.Find("ContinueButton").Find("Icon").gameObject.SetActive(false);
        }

        //Player
        playerController.transform.parent = null;
        StartCoroutine(SetActiveAfterTime(playerController.gameObject, false, 2.5f));

        gameOverPanel.transform.Find("ScoreNum").Find("Text").GetComponent<Text>().text = scoreText.text;

        if (Score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", int.Parse(scoreText.text));
            gameOverPanel.transform.Find("Best").Find("Text").GetComponent<Text>().text = "New Best";
        }
        else
        {
            gameOverPanel.transform.Find("Best").Find("Text").GetComponent<Text>().text = "Best";
        }

        gameOverPanel.transform.Find("BestNum").Find("Text").GetComponent<Text>().text = PlayerPrefs.GetInt("highscore").ToString();

        //Activate objects smoothly
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Score").gameObject, true, 0.2f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("ScoreNum").gameObject, true, 0.2f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Best").gameObject, true, 0.5f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("BestNum").gameObject, true, 0.5f));
        if (int.Parse(scoreText.text) > 0)
        {
            if (isContinue)
            {
                StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("RetryButton").gameObject, true, 1f));
            }
            else
            {
                if (PlayerPrefs.GetInt("removeAds") == 1)
                {
                    StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("RetryButton").gameObject, true, 1f));
                }
                else
                {
                    StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("RetryButton").gameObject, true, 4f));
                }

                StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("ContinueButton").gameObject, true, 0.8f));               
            }
        }
        else
        {
            StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("RetryButton").gameObject, true, 1f));
        }

        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("HomeButton").gameObject, true, 1f));        

        //Advertisement
        if (PlayerPrefs.GetInt("removeAds") == 0)
        {
            StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("NoAds").gameObject, true, 1f));
            PlayerPrefs.SetInt("countToShowAd", PlayerPrefs.GetInt("countToShowAd") + 1);

            if (PlayerPrefs.GetInt("countToShowAd") == 5)
            {
                PlayerPrefs.SetInt("countToShowAd", 0);
                StartCoroutine(unityAds.ShowVideoAd());
            }
        }
    }

    public void RestartGame()
    {
        //Reset player position
        Transform player = playerController.gameObject.transform;
        player.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
        player.GetComponent<Rigidbody2D>().simulated = false;
        player.gameObject.SetActive(true);
        playerController.resetPlayerPosition = true;
        playerController.speedToShowPlayer = 3f;
        playerController.transform.rotation = Quaternion.identity;

        HideGameOver();
        StopAllCoroutines();

        StartCoroutine(FadeAudioSource.StartFade(music, 1f, 1f));
    }

    public void Retry()
    {
        Score = 0;
        isRetry = true;

        if (isContinue)
        {
            isContinue = false;
        }

        RestartGame();
    }

    public void Continue()
    {
        isContinue = true;
        
        if (PlayerPrefs.GetInt("countToShowAd") > 0)
        {
            PlayerPrefs.SetInt("countToShowAd", PlayerPrefs.GetInt("countToShowAd") - 1);
        }        

        if (PlayerPrefs.GetInt("removeAds") == 0)
        {
            unityAds.ShowRewardedVideo();
        }
        else
        {
            RestartGame();
        }
    }

    public IEnumerator SetActiveAfterTime(GameObject gameObject, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(active);
    }

    public IEnumerator PlayAnimationAfterTime(Animator animator, string animationName, float delay, float speed = 0)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(animationName);
        if (speed > 0)
            animator.speed = speed;
        print("Finish Animation!");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenOptions()
    {
        //HideMenu();
        menu.SetActive(false);
        optionsPanel.SetActive(true);

        if (difficulty == "Easy")
            optionsPanel.transform.Find("DifficultyToggle").Find("Easy").GetComponent<Toggle>().isOn = true;
        if (difficulty == "Hard")
            optionsPanel.transform.Find("DifficultyToggle").Find("Hard").GetComponent<Toggle>().isOn = true;

        // foreach (Transform buttons in menu.transform)
        // {
        //     buttons.GetComponent<Animator>().Play("FadeIn");
        // }
    }

    public void OpenCredits()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToOptions()
    {
        creditsPanel.SetActive(false);
        OpenOptions();
        // optionsPanel.SetActive(true);
    }

    public void ChangeDifficulty(string difficulty_selected)
    {
        PlayerPrefs.SetString("difficulty", difficulty_selected);
        difficulty = difficulty_selected;
    }

    public void CallJump()
    {
        playerController.Jump();
    }
}
