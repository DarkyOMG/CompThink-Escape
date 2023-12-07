using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionTile : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Vector3 m_StartPos;
    private Tile m_HitTile;
    private Tile m_SavedTile;
    private bool m_IsSet = false;
    private AlgorithmStageController m_AlgorithmStageController;
    public AlgorithmStageController AlgorithmStageController { get => m_AlgorithmStageController;set { m_AlgorithmStageController = value; } }
    public Vector2Int[] m_moves;
    [SerializeField] private List<AudioClip> m_ClonkClipList;
    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (AlgorithmStageController.IsFlying) return;
        AudioManager.instance.PlaySFX(m_ClonkClipList[Random.Range(0, 2)]);
        if (m_SavedTile)
        {
            MarkEndTile(m_SavedTile, Color.white);
            m_AlgorithmStageController.SwapTiles(gameObject,m_SavedTile.gameObject);
            m_SavedTile.gameObject.SetActive(true);
        }
        m_IsSet = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (AlgorithmStageController.IsFlying) return;
        if (m_SavedTile)
        {
            MarkEndTile(m_SavedTile, Color.white);
            m_SavedTile.gameObject.SetActive(true);
        }
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (AlgorithmStageController.IsFlying) return;
        if (m_HitTile)
        {
            transform.SetParent(m_HitTile.transform.parent);
            transform.localPosition = m_HitTile.GetWorldPosition();
            MarkEndTile(m_HitTile, Color.blue);
            m_AlgorithmStageController.SwapTiles(m_HitTile.gameObject, gameObject);

            m_SavedTile = m_HitTile;
            m_HitTile.gameObject.SetActive(false);
            m_IsSet = true;
        }
        else
        {
            transform.position = m_StartPos;
        }

        AudioManager.instance.PlaySFX(m_ClonkClipList[Random.Range(0, 2)]);
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
    private void MarkEndTile(Tile tile, Color color)
    {
        Vector2Int temp = AlgorithmStageController.GetIndexOfElement(tile.gameObject);
        foreach(Vector2Int move in m_moves)
        {
            temp+= move;
        }

        if(temp.x < 10 && temp.y <8 && temp.x >= 0 && temp.y >= 0)
        {
            Debug.Log(temp);
            GameObject tempObject = AlgorithmStageController.GetObjectFromIndex(temp);
            Tile tempTile = tempObject.GetComponent<Tile>();
            Debug.Log(tempObject);
            if(tempTile){
                tempTile.gameObject.GetComponent<Image>().color = color;
                tempTile.m_IsBlue = color == Color.blue;
            }
        }
    }
}
