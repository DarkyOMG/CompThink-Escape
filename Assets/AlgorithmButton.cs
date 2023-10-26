using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlgorithmButton : MonoBehaviour
{
    private TMP_Text m_text;
    public int m_Index 
    { get; set; }
    private HamsterController m_Hamster;
    public HamsterController Hamster   // property
    {
        get { return m_Hamster; }   // get method
        set { m_Hamster = value; }  // set method
    }

    public void Start()
    {
        m_text = GetComponentInChildren<TMP_Text>();
        m_text.rectTransform.sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        m_text.text = m_Hamster.GetAction(m_Index);
    }
    public void ChangeSetting()
    {
        m_Hamster.SetAction(m_Index);
        m_text.text = m_Hamster.GetAction(m_Index);
    }

}
