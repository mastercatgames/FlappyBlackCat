using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ************** Flappy Black Cat ********************
public class GameController : MonoBehaviour
{
    public int Score;
    public Text scoreText;
    public AudioSource point_sfx;
    public float fadeTime = 1;
    public AudioSource music;
    void Start()
    {
        Time.timeScale = 1;
        point_sfx = GetComponent<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 1f;
        // StartCoroutine(FadeSound());
    }
    // public void FadeSound()
    // {
    //     if (fadeTime == 0)
    //     {
    //         music.volume = 0;
    //         return;
    //     }
    //     StartCoroutine(FadeSound());
    // }
    // public IEnumerator FadeSound()
    // {
    //     float t = fadeTime;
    //     while (t > 0)
    //     {
    //         yield return null;
    //         t -= Time.deltaTime;
    //         music.volume = t / fadeTime;
    //     }
    //     print("Finished Coroutine");
    //     yield break;        
    // }


}
