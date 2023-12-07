using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageController : MonoBehaviour
{
    [SerializeField] private List<ActionListSO> m_Actions;
    public void Start()
    {
        AudioManager.instance.InitAudio();
        int lastHit = 0;
        for (int i = 10; i < 14; i++)
        {
            if (RoomStateHolder.instance.GetIndexState(i))
            {
                lastHit = i;
            }
        }
        int dialogueIndex = !RoomStateHolder.instance.GetIndexState(0) ? 0 : lastHit - 9;
        ActionListEnumerator.instance.SetActionList(m_Actions[dialogueIndex]);
        ActionListEnumerator.instance.StartActionList();
    }

}
