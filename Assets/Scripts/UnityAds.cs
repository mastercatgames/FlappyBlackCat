using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.Purchasing;

public class UnityAds : MonoBehaviour
{
    private GameController gameController;
    private string gameId = "3928067";
    private bool testMode = false;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (PlayerPrefs.GetInt("removeAds") == 0)
        {
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerWhenInitialized());
        }
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show("BottomBanner");
    }

    public IEnumerator ShowVideoAd()
    {
        yield return new WaitForSeconds(0.7f);
        Advertisement.Show("video");
    }

    public void ShowRewardedVideo()
    {
        var options = new ShowOptions { resultCallback = OnUnityAdsDidFinish };
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            print("Reward the user for watching the ad to completion.");
            gameController.RestartGame();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            print("Do not reward the user for skipping the ad");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("countToShowAd", 0);
        PlayerPrefs.SetInt("removeAds", 1);
        //StopCoroutine(ShowBannerWhenInitialized());
        Advertisement.Banner.Hide();
        //SceneManager.LoadScene(0);
    }

    public void RestoreProduct(Product product)
    {
        //Calls when user reinstall the app
        if (product.definition.id == "no_ads2")
        {
            RemoveAds();
        }
    }
}
