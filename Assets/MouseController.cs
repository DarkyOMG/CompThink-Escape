using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    public bool chosenStatus = false;

    [SerializeField] private Sprite m_NormalSprite;
    [SerializeField] private Sprite m_HighlightSprite;
    [SerializeField] private Image m_MouseImage;


    public void ChangeStatus()
    {
        chosenStatus = !chosenStatus;
        EventCollector.instance.OnMouseStatusChanged.Invoke();
        Highlight();
    }
    private void Highlight()
    {
        m_MouseImage.sprite = chosenStatus? m_HighlightSprite : m_NormalSprite;
    }

}
