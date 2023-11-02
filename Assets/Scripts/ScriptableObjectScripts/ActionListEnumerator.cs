using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/ActionListEnumerator")]
public class ActionListEnumerator : SingletonScriptableObject<ActionListEnumerator>
{

    private ActionListSO m_Actionlist;
    private int m_CurrentAction;
    private int m_CurrentDialogueIndex;
    private DialogueSO m_CurrentDialogue;
    public DecompStageController decompStageController;

    public void SetActionList(ActionListSO actionlist)
    {
        m_Actionlist = actionlist;
        if(actionlist == null)
        {
            m_CurrentAction= 0;
            m_CurrentDialogueIndex= 0;
        }
    }

    public void StartActionList()
    {
        m_CurrentAction = 0;
        EventCollector.instance.OnActionExecuted += ExecuteNextAction;
        ExecuteNextAction();
    }

    private void ExecuteNextAction()
    {
        if(m_Actionlist == null || m_Actionlist.ActionList.Count <= m_CurrentAction)
        {
            EventCollector.instance.OnActionExecuted-= ExecuteNextAction;
            return;
        }

        ActionSO so = m_Actionlist.ActionList[m_CurrentAction];
        if(so.ActionType == ActionType.Dialogue)
        {
            m_CurrentDialogue = (so as DialogueSO);
            EventCollector.instance.OnDialogueTimeRanOut += IterateDialogue;
            IterateDialogue();
        }
        if(so.ActionType == ActionType.DecompRounds) 
        {
            decompStageController.StartRound(so as DecompActionSO);
            m_CurrentAction++;
        }
    }

    private void IterateDialogue()
    {
        if(m_CurrentDialogue != null && m_CurrentDialogueIndex < m_CurrentDialogue.Dialogues.Count) 
        {
            PlayNextDialogue(m_CurrentDialogue.Dialogues[m_CurrentDialogueIndex]);
            m_CurrentDialogueIndex++;
        }
        else
        {
            EventCollector.instance.OnDialogueTimeRanOut -= IterateDialogue;
            m_CurrentDialogueIndex = 0;
            m_CurrentDialogue = null;
            m_CurrentAction++;
            EventCollector.instance.OnActionExecuted.Invoke();
        }
    }
    private void PlayNextDialogue(DialogueSO.Dialogue dialogue)
    {
        DialogueTextPrinter.instance.ShowDialogueText(dialogue.DialogueText, dialogue.AudioClip.length, m_CurrentDialogueIndex == m_CurrentDialogue.Dialogues.Count-1);
        AudioManager.instance.PlaySound(dialogue.AudioClip, ClipType.Dialogue);
    }


}
