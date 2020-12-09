using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotToStore : MonoBehaviour
{
    string saveLocation = "/Documents/_Game Dev/_Arquivos de projeto/Flappy Black Cat (arquivos diversos)/Screenshots for store/";
    // Start is called before the first frame update
    void Start()
    {
        print("Press P to take a screenshot.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string file = saveLocation + Screen.width + "x" + Screen.height + "-" + timestamp + ".png";
            ScreenCapture.CaptureScreenshot(file);
            print("Screenshot saved at " + file);
        }
    }
}
