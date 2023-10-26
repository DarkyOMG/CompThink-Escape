using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NumberChangeType { PlusOne,DivideByTwo, Reset}
public class MiniRiddleHandler : MonoBehaviour
{
    private Queue<int> m_ButtonSequence = new();
    private int[] m_WinningSequence = new int[3] { 1, 2, 3 };

    [SerializeField] private List<Image> m_PizzaSlices;
    [SerializeField] private List<Sprite> m_PizzaSlicesSprites;
    private List<bool> m_PizzaSliceStatus = new List<bool>() { false, true, true, true, true, false, false, true };
    private List<bool> m_PizzaSliceCorrectStatus = new List<bool>() { true, false, true, true, false, true, false, false };

    [SerializeField] private List<Image> m_Screws;
    private List<int> m_ScrewStatus = new List<int>() { 0,0,0,0 };
    private List<int> m_ScrewCorrectStatus = new List<int>() { 2,2,3,0 };
    [SerializeField] private TMP_Text m_CalculatorText;
    private float m_CalculatorNumber;

    private bool m_PlayerHasScrewDriver = false;


    private void Start()
    {
        m_CalculatorNumber = 2500;
        m_CalculatorText.text = "2500";
        ColorSlices();

    }
    public void AddButtonToSequence(int id)
    {
        m_ButtonSequence.Enqueue(id);
        if (m_ButtonSequence.Count > 3) m_ButtonSequence.Dequeue();
        if (m_ButtonSequence.Count == 3) CheckRightSequence();

    }
    private void CheckRightSequence()
    {
        bool isRightSequence = true;
        int[] temp = m_ButtonSequence.ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (temp[i] != m_WinningSequence[i]) isRightSequence = false;
        }
        if (isRightSequence)
        {
            RoomStateHolder.instance.ChangeObjectState(0);
        }
    }

    public void ChangeNumber(int alterationType)
    {
        m_CalculatorNumber = (NumberChangeType)alterationType == NumberChangeType.DivideByTwo ? m_CalculatorNumber / 2.0f : (NumberChangeType)alterationType == NumberChangeType.PlusOne? m_CalculatorNumber + 1.0f : m_CalculatorNumber = 2500.0f;
        if(m_CalculatorNumber %1 != 0)
        {
            m_CalculatorNumber = 2500.0f;
        }
        CheckRightNumber();
        m_CalculatorText.text = $"{((int)m_CalculatorNumber).ToString()}";
    }
    private void CheckRightNumber()
    {
        if(m_CalculatorNumber == 42)
        {
            RoomStateHolder.instance.ChangeObjectState(4);
        }
    }
    private void ColorSlices()
    {
        for(int i = 0; i < 8; i++)
        {
            if (m_PizzaSliceStatus[i])
            {
                m_PizzaSlices[i].sprite = m_PizzaSlicesSprites[0];
            }
            else
            {
                m_PizzaSlices[i].sprite = m_PizzaSlicesSprites[1];
            }
        }
    }
    public void ChangeSliceColor(int i)
    {
        m_PizzaSliceStatus[i] = !m_PizzaSliceStatus[i];
        ColorSlices();
        CheckRightColoring();
    }
    private void CheckRightColoring()
    {
        bool allCorrect = true;
        for(int i = 0; i < 8; i++)
        {
            if (m_PizzaSliceStatus[i] != m_PizzaSliceCorrectStatus[i]) allCorrect = false;
        }
        if (allCorrect)
        {
            RoomStateHolder.instance.ChangeObjectState(2);
        }
    }
    public void RotateScrew(int index)
    {
        if (!m_PlayerHasScrewDriver) return;
        m_ScrewStatus[index] = (m_ScrewStatus[index]+1)%4;
        RotateScrews();
    }

    private void RotateScrews()
    {
        for (int i = 0; i < 4; i++)
        {

            m_Screws[i].transform.eulerAngles = new Vector3(0, 0, (-45 * m_ScrewStatus[i]));
        }
        CheckRightScrews();
    }
    private void CheckRightScrews()
    {
        bool allCorrect = true;
        for (int i = 0; i < 4; i++)
        {
            if (m_ScrewStatus[i] != m_ScrewCorrectStatus[i]) allCorrect = false;
        }
        if (allCorrect)
        {
            RoomStateHolder.instance.ChangeObjectState(8);
        }
    }
    public void TakeScrewdriver()
    {
        m_PlayerHasScrewDriver = true;
    }




}
