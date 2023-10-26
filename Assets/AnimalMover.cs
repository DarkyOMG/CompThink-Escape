using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimalMover : MonoBehaviour
{
    private bool m_IsMoving = false;
    private Vector3 m_Position;
    private Vector3 m_TargetPosition;
    private float m_LerpVar = 0f;
    [SerializeField] private float m_Speed = 0.5f;
    public float Speed { get => m_Speed;set { m_Speed = value; } }


    public void SetTarget(Vector3 targetPosition)
    {
        m_Position = transform.localPosition;
        m_LerpVar = 0f;
        m_TargetPosition = targetPosition;
        m_IsMoving = true;
    }
    public void Update()
    {
        if (m_IsMoving)
        {
            m_LerpVar += Time.deltaTime * m_Speed;
            m_LerpVar = m_LerpVar >= 1.0f ?  1.0f : m_LerpVar;
            transform.localPosition = Vector3.Lerp(m_Position, m_TargetPosition, m_LerpVar);
            if (transform.localPosition == m_TargetPosition)
            {
                m_IsMoving = false;
                EventCollector.instance.OnAnimalReachedEnd.Invoke();
            }
        }
    }
}
