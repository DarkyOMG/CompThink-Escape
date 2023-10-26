using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{

    private int m_CurrentViewIndex = 0;
    [SerializeField] private List<GameObject> m_views = new List<GameObject>();


    public void SwitchView(int viewIndex)
    {
        m_views[m_CurrentViewIndex].SetActive(false);
        m_CurrentViewIndex = viewIndex;
        m_views[m_CurrentViewIndex].SetActive(true);
    }

}
