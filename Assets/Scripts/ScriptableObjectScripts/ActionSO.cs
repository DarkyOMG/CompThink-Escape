using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType { Dialogue, DecompRounds}
[CreateAssetMenu(fileName = "ActionSO", menuName = "ScriptableObjects/ActionSO", order = 1)]
public class ActionSO : ScriptableObject
{
    [SerializeField] private ActionType m_ActionType;
    public ActionType ActionType { get { return m_ActionType; } } 
}
