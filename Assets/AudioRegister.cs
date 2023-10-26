using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRegister : MonoBehaviour
{
    [SerializeField] private GameObjectSO m_AudioSourceSO;
    // Start is called before the first frame update
    public void Awake()
    {
        m_AudioSourceSO.go = gameObject;   
    }

}
