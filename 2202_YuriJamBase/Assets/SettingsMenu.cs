using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MusicMixer;
    public AudioMixer SFXMixer;
    public AudioMixer VoiceMixer;

    //private void Start()
    //{
    //    float Music = PlayerPrefs.GetFloat("MusicVol", 1f);
    //    float Music2 = PlayerPrefs.GetFloat("SFXvol", 1f);
    //    float Music3 = PlayerPrefs.GetFloat("volumeV", 1f);
    //}

    public void SetVolumeM(float volumeM)
    {
        MusicMixer.SetFloat("MusicVol", volumeM);
        PlayerPrefs.SetFloat("MusicVol", volumeM);
        PlayerPrefs.Save();
    }
    public void SetVolumeSFX(float volumeSFX)
    {
        SFXMixer.SetFloat("SFXvol", volumeSFX);
        PlayerPrefs.SetFloat("SFXVol", volumeSFX);
        PlayerPrefs.Save();
    }
    public void SetVolumeV(float volumeV)
    {
        VoiceMixer.SetFloat("VoiVol", volumeV);
        PlayerPrefs.SetFloat("VoiVol", volumeV);
        PlayerPrefs.Save();
    }
    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
