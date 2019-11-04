using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public Slider backGroundSlider;
    public Slider VFXSlider;

    public AudioSource BackGroundAudioSource;
    public AudioSource PlayerAudioSource;

    public Transform ExplosionBullet;
    public Transform ExplosionLevel012;
    public Transform ExplosionLevel34;


    public void ChangeBackGroundMusicVolume()
    {
        BackGroundAudioSource.volume = backGroundSlider.value;
    }

    public void ChangeVFXSoundsVolume()
    {
        ExplosionBullet.GetComponent<AudioSource>().volume = VFXSlider.value;
        ExplosionLevel012.GetComponent<AudioSource>().volume = VFXSlider.value;
        ExplosionLevel34.GetComponent<AudioSource>().volume = VFXSlider.value;

        PlayerAudioSource.volume = VFXSlider.value;
    }
}
