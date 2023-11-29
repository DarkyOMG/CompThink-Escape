using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Pipe { A,B,C,D,E,F,G,H}
public class DebuggingStageController : MonoBehaviour
{
    [SerializeField] private ActionListSO m_Dialogue;
    [SerializeField] private List<Hole> m_Holes;
    [SerializeField] private List<TMP_Text> m_HoleTexts;
    [SerializeField] private List<Image> m_Signallights;
    [SerializeField] private List<Sprite> m_SignalSprites;
    [SerializeField] private GameObject m_WinModal;
    private int[] m_PipeStatus = new int[8];



    public void FillHole(int index, Cork cork)
    {
        cork.gameObject.SetActive(false);
        m_Holes[index].SetFilled(cork);
        m_HoleTexts[index].text = "";
        foreach (Pipe pipe in m_Holes[index].ControlledPipes)
        {
            m_HoleTexts[index].text += Enum.GetName(typeof(Pipe), pipe) + " ";
            m_PipeStatus[(int)pipe] += 1;
            m_Signallights[(int)pipe].sprite = m_SignalSprites[m_PipeStatus[(int)pipe]];
        }
        CheckWin();
    }
    public void EmptyHole(int index, Cork cork)
    {
        cork.ResetPosition();
        cork.gameObject.SetActive(true);
        foreach (Pipe pipe in m_Holes[index].ControlledPipes)
        {
            m_PipeStatus[(int)pipe] -= 1;
            m_Signallights[(int)pipe].sprite = m_SignalSprites[m_PipeStatus[(int)pipe]];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Hole hole in m_Holes)
        {
            hole.DebuggingStageController = this;
        }
        ActionListEnumerator.instance.SetActionList(m_Dialogue);
        ActionListEnumerator.instance.StartActionList();
    }
    private void CheckWin()
    {
        bool isCorrect = true;
        int[] winCondition = new int[8] { 2, 0, 2, 0, 2, 2, 0, 0 };
        for(int i = 0; i < winCondition.Length; i++)
        {
            if (winCondition[i] != m_PipeStatus[i]) isCorrect = false;
        }
        if (isCorrect)
        {
            m_WinModal.SetActive(true);
            RoomStateHolder.instance.ChangeObjectState(9);
            RoomStateHolder.instance.ChangeObjectState(13);
        }
    }
}
