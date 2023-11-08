using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueRegister : MonoBehaviour
{
    private bool m_Initialized = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(!m_Initialized){
            DialogueTextPrinter.instance.m_TextField = GetComponent<TMP_Text>();
            gameObject.SetActive(false);
            m_Initialized = true;
        }
    }

}
