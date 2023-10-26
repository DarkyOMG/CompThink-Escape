using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public enum ClipType { Dialogue, SFX, Music }
/**
 * A Class to manage Audio output by holding references to audiosources, changing volume and changing clips.
 */
[CreateAssetMenu(menuName = "Manager/AudioManager")]
public class AudioManager : SingletonScriptableObject<AudioManager>
{
    [SerializeField] public GameObjectSO m_AudioSourceMusic;
    [SerializeField] public GameObjectSO m_AudioSourceSFX;
    [SerializeField] public GameObjectSO m_AudioSourceDialogue;
    [SerializeField] private FloatSO m_AudioVolumeMusic;
    [SerializeField] private FloatSO m_AudioVolumeSFX;
    [SerializeField] private FloatSO m_AudioVolumeDialogue;
    
    // Used when a scene is loaded and adjusts the current Audio-Volume to the values specified in the pause-menu.
    public void InitAudio()
    {
        if (m_AudioSourceSFX.go)
        {
            m_AudioSourceSFX.go.GetComponent<AudioSource>().volume = m_AudioVolumeSFX.value;
        }
        if (m_AudioSourceSFX.go)
        {
            m_AudioSourceMusic.go.GetComponent<AudioSource>().volume = m_AudioVolumeMusic.value;
        }
    }
    /**
     * Playes a given clip through the Soundeffect-audiosource.
     * @param   clip    The clip to be played through the audiosource.
     */
    public void PlaySound(AudioClip clip, ClipType type)
    {
        // Catch the Audiosource of the current gameobject, holding the audiosource.
        AudioSource tempSource = type == ClipType.SFX? m_AudioSourceSFX.go.GetComponent<AudioSource>() : type == ClipType.Dialogue? m_AudioSourceDialogue.go.GetComponent<AudioSource>() : m_AudioSourceMusic.go.GetComponent<AudioSource>();
        // If it is already playing a sound, stop it.
        if (tempSource.isPlaying)
        {
            tempSource.Stop();
        }
        // Play the new clip.
        tempSource.clip = clip;
        tempSource.Play();
    }

    public void StopSound(ClipType type)
    {
        PlaySound(null, type);
    }


    // Changes the volume of the Soundeffect Audiosource. Can be used with a slider.
    public void ChangeVolumeSFX(float newVolume)
    {
        m_AudioVolumeSFX.value = newVolume;
        m_AudioSourceSFX.go.GetComponent<AudioSource>().volume = newVolume;
    }
    // Changes the volume of the Soundeffect Audiosource. Can be used with a slider.
    public void ChangeVolumeDialogue(float newVolume)
    {
        m_AudioVolumeDialogue.value = newVolume;
        m_AudioSourceDialogue.go.GetComponent<AudioSource>().volume = newVolume;
    }

    // Changes the volume of the backgroundmusic Audiosource. Can be used with a slider.
    public void ChangeVolumeMusic(float newVolume)
    {
        m_AudioVolumeMusic.value = newVolume;
        m_AudioSourceMusic.go.GetComponent<AudioSource>().volume = newVolume;
    }
}
