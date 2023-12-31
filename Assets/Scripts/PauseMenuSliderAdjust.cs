﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 
 * Component for Pause Menu canvases. Used to adjust the sliders on the pause menu to the current values.
 * Add the corresponding Sliders and Float-Scriptableobjects in the Inspector to enable adjustment. 
 */
public class PauseMenuSliderAdjust : MonoBehaviour
{
    // Sliders that need adjustment.
    [SerializeField] private Slider _mouseSens;
    [SerializeField] private Slider _audioSFX;
    [SerializeField] private Slider _audioMusic;
    [SerializeField] private Slider _audioDialogue;
    // Corresponding values, saved in a ScriptableObject
    [SerializeField] private FloatSO _mouseSensfloat;
    [SerializeField] private FloatSO _audioSFXfloat;
    [SerializeField] private FloatSO _audioMusicfloat;
    [SerializeField] private FloatSO _audioDialoguefloat;
    [SerializeField] private GameObjectSO _PauseMenuSO;

    // On Scene-load, all sliders get set to the current value for various variables, like Music volume and Mouse-Sensitivity.
    void Awake()
    {
        if (_mouseSens)
        {
            _mouseSens.value = _mouseSensfloat.value;
        }
        _PauseMenuSO.go = gameObject;
        gameObject.SetActive(false);

    }


    private void OnEnable()
    {
        _audioDialogue.value = _audioDialoguefloat.value;
        _audioSFX.value = _audioSFXfloat.value;
        _audioMusic.value = _audioMusicfloat.value;
    }
}
