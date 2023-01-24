using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    [SerializeField] private SaveSerial _saver;
    [SerializeField] private Material[] gradientBackGrounds;

    public void ChangeSkyboxToNext(int next=1)
    {
        _saver.currentSkyboxIdx = (_saver.currentSkyboxIdx + next) % gradientBackGrounds.Length;
        RenderSettings.skybox = gradientBackGrounds[_saver.currentSkyboxIdx];
    }
}
