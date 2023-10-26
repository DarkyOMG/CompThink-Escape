using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionTile : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector3 m_StartPos;
    private Tile m_HitTile;
    private Tile m_SavedTile;
    private bool m_IsSet = false;
    private AlgorithmStageController m_AlgorithmStageController;
    public AlgorithmStageController AlgorithmStageController { get => m_AlgorithmStageController;set { m_AlgorithmStageController = value; } }
    public Vector2Int[] m_moves;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_SavedTile)
        {
            m_AlgorithmStageController.SwapTiles(gameObject,m_SavedTile.gameObject);
            m_SavedTile.gameObject.SetActive(true);

        }
        m_IsSet = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_HitTile)
        {
            transform.SetParent(m_HitTile.transform.parent);
            transform.localPosition = m_HitTile.GetWorldPosition();
            m_AlgorithmStageController.SwapTiles(m_HitTile.gameObject, gameObject);
            m_SavedTile = m_HitTile;
            m_HitTile.gameObject.SetActive(false);
            m_IsSet = true;
        }
        else
        {
            transform.position = m_StartPos;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_IsSet) return;
        Tile temp = collision.GetComponent<Tile>();
        if (temp && temp.type == TileType.Ground)
        {
            m_HitTile?.Highlight(false);
            m_HitTile = temp;
            temp.Highlight(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_HitTile || m_IsSet) return;
        if (collision.gameObject.Equals(m_HitTile.gameObject))
        {
            m_HitTile.Highlight(false);
            m_HitTile = null;
        }
    }
    private void OnDestroy()
    {
        if (m_SavedTile)
        {
            Destroy(m_SavedTile.gameObject);
        }
    }
}
