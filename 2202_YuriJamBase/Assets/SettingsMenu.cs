using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MusicMixer;
    public AudioMixer SFXMixer;
    public AudioMixer VoiceMixer;
    public void SetVolumeM(float volumeM)
    {
        MusicMixer.SetFloat("MusicVol", volumeM);
    }
    public void SetVolumeSFX(float volumeSFX)
    {
        SFXMixer.SetFloat("SFXvol", volumeSFX);
    }
    public void SetVolumeV(float volumeV)
    {
        VoiceMixer.SetFloat("VoiVol", volumeV);
    }
    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
