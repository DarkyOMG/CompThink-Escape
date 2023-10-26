using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum DecompState { Ready, phaseone,phasetwo}
public class DecompStageController : MonoBehaviour
{

    private List<Tuple<AnimalMover,MouseController>> m_Mice = new List<Tuple<AnimalMover,MouseController>>();
    [SerializeField] private Image m_TargetImage;
    [SerializeField] private TMP_Text m_TargetText;
    [SerializeField] private Transform m_MouseSpawnPoint;
    [SerializeField] private Transform m_MouseCorrectEndPoint;
    [SerializeField] private Transform m_MouseWrongEndPoint;
    [SerializeField] private GameObject m_MousePrefab;
    [SerializeField] private GameObject m_MouseParent;
    [SerializeField] private DecompScoreHolder m_ScoreHolder;
    [SerializeField] private ColorFlash m_ColorFlash;
    [SerializeField] private GameObject m_WinModal;
    private DecompState m_State = DecompState.Ready;



    private List<Vector3> m_MousePositions = new List<Vector3>{
        new Vector3(0,50,0),
        new Vector3(100,-50,0),
        new Vector3(-100,-50,0),
        new Vector3(200,-100,0),
        new Vector3(-200,-100,0) };

    [SerializeField] private Button m_EndRoundButton;
    private DecompActionSO m_CurrentAction;


    private void OnEnable()
    {
        EventCollector.instance.OnMouseStatusChanged += UpdateAfterMouseChange;
        EventCollector.instance.OnAnimalReachedEnd += FinalizeAfterMouseMovement;
    }
    private void OnDisable()
    {
        EventCollector.instance.OnMouseStatusChanged -= UpdateAfterMouseChange;
        EventCollector.instance.OnAnimalReachedEnd -= FinalizeAfterMouseMovement;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_State = DecompState.Ready;
        ActionListEnumerator.instance.decompStageController = this;
        for(int i = 0; i < 5; i++)
        {
            GameObject mouse = Instantiate(m_MousePrefab, m_MouseSpawnPoint.position,Quaternion.identity,m_MouseParent.transform);
            m_Mice.Add(new Tuple<AnimalMover,MouseController>(mouse.GetComponent<AnimalMover>(),mouse.GetComponent<MouseController>()));
        }
        m_TargetImage.gameObject.SetActive(false);
        m_TargetText.gameObject.SetActive(false);
        UpdateAfterMouseChange();
    }

    public void StartRound(DecompActionSO decompActionSO)
    {
        m_State = DecompState.phaseone;
        m_EndRoundButton.gameObject.SetActive(true);
        m_CurrentAction = decompActionSO;
        if(decompActionSO.type == DecompRoundType.Image)
        {
            m_TargetImage.sprite = decompActionSO.targetImage;
            m_TargetImage.gameObject.SetActive(true);
            m_TargetText.gameObject.SetActive(false);

            for (int i = 0;i<5;i++)
            {
                m_Mice[i].Item1.gameObject.transform.localPosition = m_MouseSpawnPoint.localPosition;
                if (m_Mice[i].Item2.chosenStatus) m_Mice[i].Item2.ChangeStatus();
                m_Mice[i].Item2.image.sprite = decompActionSO.imagePartList[i];
                m_Mice[i].Item2.text.gameObject.SetActive(false);
                m_Mice[i].Item2.image.gameObject.SetActive(true);
                m_Mice[i].Item1.SetTarget(m_MousePositions[i]);
            }

        }
        if (decompActionSO.type == DecompRoundType.Text)
        {
            m_TargetText.text = decompActionSO.targetText;
            m_TargetImage.gameObject.SetActive(false);
            m_TargetText.gameObject.SetActive(true);

            for (int i = 0; i < 5; i++)
            {
                m_Mice[i].Item1.gameObject.transform.localPosition = m_MouseSpawnPoint.localPosition;
                if (m_Mice[i].Item2.chosenStatus) m_Mice[i].Item2.ChangeStatus();
                m_Mice[i].Item2.text.text = decompActionSO.textPartList[i];
                m_Mice[i].Item2.image.gameObject.SetActive(false);
                m_Mice[i].Item2.text.gameObject.SetActive(true);
                m_Mice[i].Item1.SetTarget(m_MousePositions[i]);
            }

        }
    }
    public void UpdateAfterMouseChange()
    {
        int miceClicked = 0;
        foreach(Tuple<AnimalMover,MouseController> mouse in m_Mice)
        {
            miceClicked = mouse.Item2.chosenStatus ? miceClicked + 1 : miceClicked;
        }
        m_EndRoundButton.interactable = miceClicked == 3;
    }
    public void EndRound()
    {
        m_State = DecompState.phasetwo;
        HashSet<int> chosenMice = new HashSet<int>();
        for (int i = 0;i<5;i++)
        {
            if (m_Mice[i].Item2.chosenStatus)
            {
                chosenMice.Add(i);
                m_Mice[i].Item1.SetTarget(m_MouseCorrectEndPoint.localPosition);
            }
            else
            {
                m_Mice[i].Item1.SetTarget(m_MouseWrongEndPoint.localPosition);
            }

        }
        m_ScoreHolder.ChangeScore(chosenMice.SetEquals(m_CurrentAction.correctIndices));
        m_ColorFlash.FlashColor(chosenMice.SetEquals(m_CurrentAction.correctIndices) ? Color.green : Color.red);
        if(m_ScoreHolder.Score >= 5)
        {
            m_WinModal.gameObject.SetActive(true);
            RoomStateHolder.instance.ChangeObjectState(1);
        }
        
    }
    public void FinalizeAfterMouseMovement()
    {
        if(m_State == DecompState.phasetwo)
        {
            EventCollector.instance.OnActionExecuted?.Invoke();
        }
    }


}
