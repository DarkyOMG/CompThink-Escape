using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "Manager/DialogueTextPrinter")]
public class DialogueTextPrinter : SingletonScriptableObject<DialogueTextPrinter>
{
    public TMP_Text m_TextField;
    private TimeManager.TimeTrigger m_CurrentTimeTrigger;
    [SerializeField] private GameObjectSO m_ShieldSO;
    private bool m_IsAtLastPiece = false;
    

    public void ShowDialogueText(string text, float timelength,bool lastPiece)
    {
        m_IsAtLastPiece = lastPiece;
        if(m_CurrentTimeTrigger != null)
        {
            TimeManager.Instance.RemoveTimeTrigger(m_CurrentTimeTrigger);
        }
        if (m_IsAtLastPiece)
        {
            EventCollector.instance.OnDialogueTimeRanOut += ResetAndCloseDialogue;
        }
        m_ShieldSO.go.SetActive(true);
        m_TextField.text= text;
        m_TextField.gameObject.SetActive(true);
        m_CurrentTimeTrigger = TimeManager.Instance.GetTimer(timelength, EventCollector.instance.OnDialogueTimeRanOut);
    }
    public void SkipDialogue()
    {
        TimeManager.Instance.RemoveTimeTrigger(m_CurrentTimeTrigger);
    }
    
    public void ResetAndCloseDialogue()
    {
        if(m_IsAtLastPiece)
        {
            m_TextField.gameObject.SetActive(false);
            m_TextField.transform.parent.gameObject.SetActive(false);
            m_TextField.text= string.Empty;
            EventCollector.instance.OnDialogueTimeRanOut -= ResetAndCloseDialogue;
            m_CurrentTimeTrigger= null;
            m_ShieldSO.go.SetActive(false);
        }
    }

}
