using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropAnimation : MonoBehaviour
{
    public float m_Speed = 2.0f;
    private float m_LerpVal = 0f;
    private Vector3 m_TargetPos;
    private Vector3 m_Position;
    private bool m_IsMoving;
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 475.0f, 0);
        m_Position= transform.localPosition;
        m_TargetPos= new Vector3(transform.localPosition.x, 0.0f, 0);
        m_LerpVal = 0f;
        m_IsMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsMoving)
        {     
            float tempSpeed = m_LerpVal < 0.7f? (1 - m_LerpVal) * m_Speed:0.3f * m_Speed;
            m_LerpVal += Time.deltaTime * tempSpeed;
            m_LerpVal = m_LerpVal >= 1.0f ? 1.0f : m_LerpVal;
            transform.localPosition = Vector3.Lerp(m_Position, m_TargetPos, m_LerpVal);
            if(m_LerpVal >=1.0f) m_IsMoving= false;
        }
    }
}
