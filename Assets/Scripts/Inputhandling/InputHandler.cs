using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Enum to distinguish game-modes for inputhandling.
public enum GameInputType { Menu }

/**
 * Class to centrally handle inputs. 
 * Contains all inputbehaviour and activates or deactivates input-behaviour depending on the current game-mode.
 * Uses Singleton pattern to only have one input-handling throughout the game.
 */ 
[CreateAssetMenu(menuName = "Manager/InputHandler")]
public class InputHandler : SingletonScriptableObject<InputHandler>
{

    // Reference to unity-generated input-behaviour-script.
    private InputMasterCompSys m_Inputs;

    // Current Gametype used for input-behaviour-switching.
    [SerializeField] private GameInputType m_CurrentGIT;

    // General references for inputbehaviour on multiple scenes. 
    [SerializeField] private GameObjectSO selected;
    [SerializeField] private GameObjectSO m_Camera;
    [SerializeField] private GridSO _mainGrid;
    [SerializeField] private GridSO _auxGrid;
    [SerializeField] private GameObjectSO m_Pausemenu;
    public AudioClip[] clips = new AudioClip[5];

    
    // On Load, reset all functions to the current game mode and reset variables to avoid currupt states.
    public void OnEnable()
    {
        SetFunctions();
    }
    // On Disable, reset all input-behaviour.
    public void OnDisable()
    {
        ResetFunctions();
    }

    // Function to change the Gametype. This must be called by the new gamemode to deactivate old input-behaviour and activate current input-behaviour.
    public void ChangeGIT(GameInputType targetType)
    {
        ResetFunctions();
        this.m_CurrentGIT = targetType;
        SetFunctions();
    }

    /**
     * Activates Input-behaviour depending on the current Game-Mode. 
     * The Input-Actionmap must contain all needed entrys and all methods used on these actions must be added to the corresponding event.
     * Actionmap GateBuilder contains the ACtion ClickAction, which triggers on left-mouse-button click. 
     * On this Action, the Method PlaceSelected will be added, to be invoked on the ClickAction-Event.
     * The GateBuilder-Actionmap will be activated, if the current Gamemode (GameInputType) is GateBuilder.
     */
    private void SetFunctions()
    {
        // Get a Inputmaster, if there is none.
        if(m_Inputs == null)
        {
            m_Inputs = new InputMasterCompSys();
        }

        if(m_CurrentGIT == GameInputType.Menu)
        {
            m_Inputs.General.Enable();
            m_Inputs.General.Cancel.performed += Cancel;
        }
    }
    /*
     * Reset the Inputmaster so that there is no Inputbehaviour activated and all delegates are removed.
     * Is used on Scene-Switches to change Input-behaviour on different scenes.
     */
    private void ResetFunctions()
    {
        if (m_CurrentGIT == GameInputType.Menu)
        {
            m_Inputs.General.Disable();
            m_Inputs.General.Cancel.performed -= Cancel;
        }
    }

    /*
     * Cancel method, to either present the Pause menu, or stop placing a logic element on Powercity.
     * @param   context Information provided by the triggering event. In this case the buttonpress on the ESC-key.
     */
    private void Cancel(InputAction.CallbackContext context)
    {
        // In any other case, pause the game.
        PauseGame();
    }



    public void PauseGame()
    {
        // If the game is paused and the pause menu is shown, hide it and unpause.
        if (m_Pausemenu.go.activeSelf)
        {
            Time.timeScale = 1.0f;
            m_Pausemenu.go.SetActive(false);
        }

        // If the game is not paused, show the pause menu and stop movement.
        else
        {
            Time.timeScale = 0f;
            m_Pausemenu.go.SetActive(true);
        }
    }
}
