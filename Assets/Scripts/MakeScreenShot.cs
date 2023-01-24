using System;
using UnityEngine;

public class MakeScreenShot : MonoBehaviour
{
    [SerializeField] private Canvas _mainCanvas;

    public void DoScreenShot()
    {
        string fileName = "Screenshot_" + DateTime.UtcNow.ToString("yyyyMMdd_hhmmss") + ".png";
        
        _mainCanvas.gameObject.SetActive(false);
        ScreenCapture.CaptureScreenshot(fileName);
        _mainCanvas.gameObject.SetActive(true);
    }
}
