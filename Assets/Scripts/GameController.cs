using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ************** Flappy Black Cat ********************
public class GameController : MonoBehaviour
{
    private PlayerController playerController;
    private SpawnPipes spawnPipes;

    [Header("=== UI ===")]
    public int Score;
    public Text scoreText;
    public Text TapToFlyText;
    public Text ReadyText;
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
    public List<string> readyRamdomTexts;
    
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnPipes = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("SpawnVacuums").GetComponent<SpawnPipes>();
        
        Time.timeScale = 1;

        point_sfx = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        ShowMenu();
        SetReadyRamdomTexts();
    }

    private void SetReadyRamdomTexts()
    {
        readyRamdomTexts = new List<string>();
        readyRamdomTexts.Add("Enjoy");
        readyRamdomTexts.Add("Thanks for playing");
    }    

    public void StartGame()
    {
        //menu.SetActive(false);        
        //menu.GetComponent<Animator>().Play("FadeOut_up_to_down");
        HideMenu();
        // firstTapButton.SetActive(true);
        // gameplayText.gameObject.SetActive(true);        
        // gameplayText.GetComponent<Animator>().Play("FadeIn_down_to_up");

        ReadyText.gameObject.SetActive(true);   
        ReadyText.text = readyRamdomTexts[Random.Range(0,2)];     
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
        spawnVacuums.SetActive(true);        
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

        // if (playerController.showingPlayer)
        // {
        //     playerController.speedToShowPlayer = 10f;
        // }
    }

    public void GameOver()
    {
        StartCoroutine(FadeAudioSource.StartFade(music, 3f, 0f));
        gameOverPanel.SetActive(true);
        jumpButton.SetActive(false);
        //scoreText.gameObject.SetActive(false);
        scoreText.GetComponent<Animator>().Play("FadeOut");
        scoreText.GetComponent<Animator>().speed = 3f;

        //Time.timeScale = 0;

        spawnPipes.CancelInvoke();

        //Player
        playerController.transform.parent = null;
        StartCoroutine(SetActiveAfterTime(playerController.gameObject, false, 2.5f));

        // float delayToShow = 1f;
        //Hide All objects in panel
        // foreach (Transform child in gameOverPanel.transform)
        // {
        //     // child.gameObject.SetActive(false);
        //     // if (child.gameObject.name.Contains("Best"))
        //     //     delayToShow = 1.5f;
        //     // else if (child.gameObject.name.Contains("ContinueButton"))
        //     //     delayToShow = 2f;
        //     // else if (child.gameObject.name.Contains("RetryButton"))
        //     //     delayToShow = 4f;

        //     StartCoroutine(SetActiveAfterTime(child.gameObject, true, 0.5f/*delayToShow*/));
        // }

        gameOverPanel.transform.Find("ScoreNum").Find("Text").GetComponent<Text>().text = scoreText.text;

        //Activate objects smoothly
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Score").gameObject, true, 0.2f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("ScoreNum").gameObject, true, 0.2f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Best").gameObject, true, 0.5f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("BestNum").gameObject, true, 0.5f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("ContinueButton").gameObject, true, 0.8f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("RetryButton").gameObject, true, 3f));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 1f;
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

}
