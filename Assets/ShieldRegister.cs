using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldRegister : MonoBehaviour
{

    [SerializeField] private GameObjectSO m_ShieldSO;
    private bool m_Initialized = false;

    public void OnEnable(){
        if(!m_Initialized){
            m_ShieldSO.go = gameObject;
            gameObject.SetActive(false);
            m_Initialized = true;
        }
    }
}
