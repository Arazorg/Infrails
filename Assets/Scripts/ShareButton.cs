using System.Collections;
using System.IO;
using UnityEngine;

public class ShareButton : MonoBehaviour
{
    private const string ShareMessageKey = "ShareMessage";

    public IEnumerator Share()
    {
        string shareMessage = LocalizationManager.GetLocalizedText(ShareMessageKey);
        yield return StartCoroutine(TakeScreenshotAndShare(shareMessage));
    }

    private IEnumerator TakeScreenshotAndShare(string shareMessage)
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "sharedScreenshot.png");
        File.WriteAllBytes(filePath, screenshot.EncodeToPNG());

        Destroy(screenshot);

        new NativeShare().AddFile(filePath).SetSubject(string.Empty).SetText(shareMessage).Share();
    }
}
