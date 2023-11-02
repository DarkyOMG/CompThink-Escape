using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageController : MonoBehaviour
{
    [SerializeField] private List<ActionListSO> m_Actions;
    [SerializeField] private GameObject m_Shield;
    public void Start()
    {
        m_Shield.SetActive(true);
       
        int lastHit = 0;
        for (int i = 10; i < 14; i++)
        {
            if (RoomStateHolder.instance.GetIndexState(i))
            {
                lastHit = i;
            }
        }
        int dialogueIndex = !RoomStateHolder.instance.GetIndexState(0) ? 0 : lastHit - 9;
        EventCollector.instance.OnActionExecuted += DisableShield;
        if (dialogueIndex < 0 || dialogueIndex > 4) { DisableShield(); return; }
        ActionListEnumerator.instance.SetActionList(m_Actions[dialogueIndex]);
        ActionListEnumerator.instance.StartActionList();
        return;
    }
    public void DisableShield()
    {
        m_Shield.SetActive(false);
        EventCollector.instance.OnActionExecuted -= DisableShield;
    }
}
