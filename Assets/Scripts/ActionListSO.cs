using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionListSO", menuName = "ScriptableObjects/ActionListSO", order = 1)]
public class ActionListSO : ScriptableObject 
{
    [SerializeField] private List<ActionSO> m_ActionList = new List<ActionSO>();
    public List<ActionSO> ActionList => m_ActionList;

}
