using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Initializer : MonoBehaviour
{

    [SerializeField] private GameObjectSO m_MainCameraSO;

    [SerializeField] private GameObjectSO m_PauseMenuSO;
    [SerializeField] private GameObject m_DialogueTextField;
    [SerializeField] private GameObject m_PauseMenu;


    // On Awake, saying the scene is loaded, all Objects are set to their ScriptableObjectVariables.
    private void Awake()
    {
        m_MainCameraSO.go = this.gameObject;
        DialogueTextPrinter.instance.m_TextField = m_DialogueTextField.GetComponent<TMP_Text>();
        AudioManager.instance.InitAudio();
        m_PauseMenuSO.go = m_PauseMenu;
        ActionListEnumerator.instance.SetActionList(null);
    }

}
