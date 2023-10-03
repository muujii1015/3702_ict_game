using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check for user input (e.g., pressing a specific key)
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Capture the screenshot and specify the filename
            string filename = "screenshot.png";
            ScreenCapture.CaptureScreenshot(filename);

            // Optionally, you can also provide the path to save the screenshot
            // For example:
            // string path = System.IO.Path.Combine(Application.dataPath, filename);
            // ScreenCapture.CaptureScreenshot(path);

            Debug.Log("Screenshot captured: " + filename);
        }
    }
}
