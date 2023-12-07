using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DecompScoreHolder : MonoBehaviour
{
    private int m_Score = 0;
    private TMP_Text m_ScoreText;

    public int Score => m_Score;


    private void Awake()
    {
        m_ScoreText = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        AudioManager.instance.InitAudio();
        m_ScoreText.text = $"{m_Score.ToString()} / 5";
    }
    public void ChangeScore(bool answeredCorrectly)
    {
        m_Score = answeredCorrectly ? m_Score + 1 : 0;
        m_ScoreText.text = $"{m_Score.ToString()} / 5";
    }
}
