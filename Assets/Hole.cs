using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    [SerializeField] private int m_Index;
    [SerializeField] List<Sprite> m_HoleSprites;
    public int Index => m_Index;
    [SerializeField] private List<Pipe> m_ControlledPipes = new List<Pipe>();
    public List<Pipe> ControlledPipes => m_ControlledPipes;
    private Cork m_FillingCork;
    private Button m_Button;
    private DebuggingStageController m_DebuggingStageController;
    public DebuggingStageController DebuggingStageController { get => m_DebuggingStageController; set { m_DebuggingStageController = value; } }


    public void Highlight()
    {

    }

    public void SetFilled(Cork cork)
    {
        m_FillingCork = cork;
        m_Button.interactable = true;
        GetComponent<Image>().sprite = m_HoleSprites[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Button = GetComponent<Button>();
    }

    public void EmptyHole()
    {
        m_DebuggingStageController.EmptyHole(m_Index,m_FillingCork);
        m_FillingCork = null;
        GetComponent<Image>().sprite = m_HoleSprites[0];
    }
}
