using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cork : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Hole m_HitHole;
    [SerializeField] private AudioClip m_PickupClip;
    private Vector3 m_StartPos;
    [SerializeField] private Sprite m_standardImage;

    [SerializeField] private DebuggingStageController m_DebuggingStageController;
    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(m_PickupClip);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 temp = new Vector3(Mathf.Clamp(Input.mousePosition.x, 50, Screen.width-50), Mathf.Clamp(Input.mousePosition.y, 50, Screen.height-50), 0f);
        transform.position = temp;
        if (m_HitHole)
        {
            m_DebuggingStageController.FillHole(m_HitHole.Index, this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
    }
    public void OnEnable()
    {

        GetComponent<Image>().sprite = m_standardImage;
    }

    public void ResetPosition()
    {
        transform.position = m_StartPos;
        m_HitHole = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hole temp = collision.GetComponent<Hole>();
        if (!temp || temp.IsFilled) return;
        if (temp) m_HitHole = temp;
        temp.Highlight(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Hole temp = collision.GetComponent<Hole>();
        if (!temp || temp.IsFilled) return;
        temp.Highlight(false);
        if (!m_HitHole) return;
        if(collision.gameObject.Equals(m_HitHole.gameObject))
        {
            m_HitHole = null;
        }
    }
}
