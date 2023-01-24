using UnityEngine;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour
{
    [SerializeField] private AudioClip cutClip;
    [SerializeField] private AudioClip perfectClip;
    [SerializeField] private AudioClip longClip;
    [SerializeField] private AudioClip buttonClip;

    [SerializeField, Range(0f, 1f)] private float cutVolume;
    [SerializeField, Range(0f, 1f)] private float perfectVolume;
    [SerializeField, Range(0f, 1f)] private float longVolume;

    [SerializeField, Range(0.5f, 3f)] private float cutPitch = 1f;
    [SerializeField, Range(0.5f, 3f)] private float perfectPitch = 1f;
    [SerializeField, Range(0.5f, 3f)] private float longPitch = 1f;
    
    public Slider volumeSlider;
    
    private bool _isPlayMusic = true;
    private bool _isVibrate = true;
    private float _volumeMult = 1f;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVibrate() => _isVibrate = !_isVibrate;
    public void ChangePlayMusic() => _isPlayMusic = !_isPlayMusic;
    public void ChangeVolume() => _volumeMult = volumeSlider.normalizedValue;
    
    public void PlayCutAndVibrate() {
        PlayClip(cutClip, cutVolume, cutPitch * Random.Range(0.8f, 1.2f), _isVibrate);
    }
    
    public void PlayPerfect(int perfectCombo) {
        PlayClip(perfectClip, perfectVolume, perfectPitch + 0.1f * Mathf.Min(perfectCombo, 5));
    }
    
    public void PlayLong() {
        PlayClip(longClip, longVolume, longPitch);
    }

    public void PlayButtonClick() {
        PlayClip(buttonClip);
    }
    
    public void Vibrate() => Handheld.Vibrate();
    
    private void PlayClip(AudioClip audioClip, float volume=1f, float pitch=1f, bool doVibrate=false)
    {
        if (_isPlayMusic)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume * _volumeMult;
            _audioSource.pitch = pitch;
            _audioSource.Play();
        }
        if (doVibrate) Vibrate();
    }
}
