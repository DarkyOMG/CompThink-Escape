using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmButton : MonoBehaviour
{
    private Image m_ButtonImage;
    [SerializeField] List<Sprite> m_ImageList;
    
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
        m_ButtonImage = GetComponent<Image>();
        m_ButtonImage.sprite = m_ImageList[m_Hamster.GetAction(m_Index)];
    }
    public void ChangeSetting()
    {
        m_Hamster.SetAction(m_Index);
        m_ButtonImage.sprite = m_ImageList[m_Hamster.GetAction(m_Index)];
    }

}
