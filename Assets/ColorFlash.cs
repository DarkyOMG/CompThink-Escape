using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorFlash : MonoBehaviour
{


    private float m_LerpVal;
    private float m_LerpSpeed = 3;
    private Image m_image;
    private bool m_IsFading = false;

    private Color m_StartColor;
    private Color m_EndColor;
    private Color m_CurrentColor;

    private void Start()
    {
        m_LerpVal = 0;
        m_image = GetComponent<Image>();
        Color temp = Color.green; temp.a = 0;
        m_image.color= temp;
    }

    private void Update()
    {
        if(m_IsFading) 
        { 
            m_LerpVal += Time.deltaTime * m_LerpSpeed;
            m_LerpVal = m_LerpVal > 1 ? 1 : m_LerpVal;
            m_CurrentColor = m_LerpVal < 0.5f ? Color.Lerp(m_StartColor, m_EndColor, m_LerpVal) : Color.Lerp(m_EndColor, m_StartColor, m_LerpVal);
            m_image.color = m_CurrentColor;
            if(m_LerpVal >=1) m_IsFading =false;
        }
    }

    public void FlashColor(Color color)
    {
        m_StartColor = color;
        m_StartColor.a = 0;
        m_EndColor = m_StartColor;
        m_EndColor.a = 1f;
        m_IsFading = true;
        m_LerpVal= 0;
    }




}
