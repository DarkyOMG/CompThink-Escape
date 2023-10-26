using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmStageController : MonoBehaviour
{


    private Grid m_Grid;
    [SerializeField] private List<GameObject> m_ActionTilePrefabs;
    [SerializeField] private Transform m_GridParent;
    [SerializeField] private Transform m_Canvas;
    private stage m_Currentstage = stage.One;
    [SerializeField] GameObject m_WinModal;
    private List<int[]> m_CurrentLabyrinth;
    [SerializeField] private Bird m_Bird;
    private Vector2Int[] m_CurrentMoveList;
    private int m_CurrentMoveInList = 0;
    [SerializeField] private List<GameObject> m_Parts;
    private List<Vector2Int> m_StartingPoints = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(0, 0), new Vector2Int(0, 0) };
    private List<int[]> m_labyrinthOne = new List<int[]>
    {
        new int[10] {0,0,0,0,1,0,0,0,0,0},
        new int[10] {1,0,0,0,0,1,0,0,1,0},
        new int[10] {0,0,1,0,0,0,0,0,0,0},
        new int[10] {0,0,1,0,0,0,1,0,0,0},
        new int[10] {0,0,0,1,0,0,0,0,1,0},
        new int[10] {0,0,0,0,1,0,0,0,0,0},
        new int[10] {0,1,0,0,0,0,0,0,0,1},
        new int[10] {0,0,0,0,1,0,1,0,0,0}
        
    };
    private List<int[]> m_labyrinthTwo = new List<int[]>
    {
        new int[10] {0,0,0,0,1,1,0,0,0,0},
        new int[10] {0,0,1,0,0,1,1,0,0,0},
        new int[10] {1,0,0,1,0,0,1,0,1,0},
        new int[10] {0,1,0,0,1,0,0,1,0,1},
        new int[10] {0,0,0,1,0,0,0,0,0,1},
        new int[10] {0,0,1,0,1,1,0,1,0,0},
        new int[10] {0,1,0,0,1,0,0,0,1,0},
        new int[10] {1,0,0,1,0,0,1,1,0,1}
    };
    private List<int[]> m_labyrinthThree = new List<int[]>
    {
        new int[10] {0,0,0,0,0,0,1,0,0,0},
        new int[10] {0,0,0,0,1,0,0,0,1,0},
        new int[10] {1,1,1,0,1,0,0,1,0,0},
        new int[10] {0,0,0,0,0,0,0,0,0,1},
        new int[10] {1,0,0,0,0,1,0,0,1,0},
        new int[10] {0,0,0,1,0,0,1,1,0,0},
        new int[10] {0,0,1,1,1,0,0,1,0,1},
        new int[10] {0,0,0,0,0,0,1,0,0,0}
    };
    private List<int> m_LabOneActionTiles = new List<int>() {0,1,5,0 };
    private List<int> m_LabTwoActionTiles = new List<int>() {1,0,3,5,2};
    private List<int> m_LabThreeActionTiles = new List<int>() {4,3,0,5,2,1 };
    private List<Vector3> m_ActionTilePositions = new List<Vector3>(){
        new Vector3(400+585, 200+305),
        new Vector3(500+585, 200+305),
        new Vector3(400+585, 100+305),
        new Vector3(500+585, 100+305),
        new Vector3(400+585, 0+305),
        new Vector3(500+585, 0+305)
        };
    [SerializeField] private ColorFlash m_ColorFlash;
    // Start is called before the first frame update
    void Start()
    {
        CreateLabyrinth();
        ResetBird();
    }

    private void CreateLabyrinth()
    {
        m_CurrentLabyrinth = m_Currentstage == stage.One ? m_labyrinthOne : m_Currentstage == stage.Two ? m_labyrinthTwo : m_labyrinthThree;
        
        m_Grid = new Grid(10, 8, 80, m_GridParent);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject go = m_Grid.InsertElement(i, j, m_Parts[(m_CurrentLabyrinth[j][i])%2]);
                go.GetComponent<Tile>().AlgorithmStageController = this;
            }
        }
        List<int> actionTileList = m_Currentstage == stage.One ? m_LabOneActionTiles : m_Currentstage == stage.Two ? m_LabTwoActionTiles : m_LabThreeActionTiles;
        for (int i = 0; i < actionTileList.Count; i++)
        {
            GameObject go = Instantiate(m_ActionTilePrefabs[actionTileList[i]], m_GridParent);
            go.transform.localPosition = m_ActionTilePositions[i];
            go.GetComponent<ActionTile>().AlgorithmStageController = this;
        }

    }

    public Vector3 GetPositionOfElement(GameObject element)
    {
        Vector2Int temp = m_Grid.GetIndexByElement(element);
        Vector3 position = m_Grid.GetPositionOfElement(temp.x, temp.y);
        return position;
    }

    public void SwapTiles(GameObject oldTile, GameObject newTile)
    {
        Vector2Int temp = m_Grid.GetIndexByElement(oldTile);
        m_Grid.PlaceTile(temp.x, temp.y, newTile);
    }

    public void StartBird()
    {
        m_Bird.transform.SetParent(m_GridParent);
        m_Bird.transform.localPosition = m_Grid.GetPositionOfElement(m_StartingPoints[0].x, m_StartingPoints[0].y);
        m_Bird.m_Position = m_StartingPoints[0];
        EventCollector.instance.OnAnimalReachedEnd += NextMove;
        NextMove();
    }

    public void NextMove()
    {
        bool hitObstacle = CheckForObstacle();
        if (hitObstacle)
        {
            ResetBird();
            m_ColorFlash.FlashColor(Color.red);
            EventCollector.instance.OnAnimalReachedEnd -= NextMove;
            return;
        }
        if(m_CurrentMoveList != null)
        {
            Vector2Int target = m_Bird.m_Position + m_CurrentMoveList[m_CurrentMoveInList];
            if (target.y > 7)
            {
                HandleWin();
                return;
            }
            m_Bird.GetComponent<AnimalMover>().SetTarget(m_Grid.GetPositionOfElement(target.x, target.y));
            m_Bird.m_Position = target;
            m_CurrentMoveInList++;
            if(m_CurrentMoveInList >= m_CurrentMoveList.Length)
            {
                m_CurrentMoveList = null;
                m_CurrentMoveInList = 0;
            }
        } else
        {
            ActionTile temp = m_Grid.GetElementAtIndex(m_Bird.m_Position.x, m_Bird.m_Position.y).GetComponent<ActionTile>();
            if (temp)
            {
                m_CurrentMoveList = temp.m_moves;
                NextMove();
            } 
            else
            {
                ResetBird();
                m_ColorFlash.FlashColor(Color.red);
                EventCollector.instance.OnAnimalReachedEnd -= NextMove;
            }
        }
    }

    private void ResetBird()
    {
        m_Bird.transform.SetParent(m_GridParent);
        m_Bird.transform.localPosition = m_Grid.GetPositionOfElement(m_StartingPoints[0].x, m_StartingPoints[0].y);
        Vector3 temp = m_Bird.transform.localPosition;
        temp.y -= 50;
        m_Bird.transform.localPosition = temp;
        m_Bird.transform.SetAsLastSibling();
        m_CurrentMoveList = null;
        m_CurrentMoveInList = 0;
    }

    private bool CheckForObstacle()
    {
        Tile temp = m_Grid.GetElementAtIndex(m_Bird.m_Position.x, m_Bird.m_Position.y).GetComponent<Tile>();
        if (!temp) return false;
        else
        {
            return temp.type == TileType.Obstacle;
        }
    }
    private void HandleWin()
    {
        m_ColorFlash.FlashColor(Color.green);

        EventCollector.instance.OnAnimalReachedEnd -= NextMove;
        if (m_Currentstage == stage.Three)
        {
            m_WinModal.SetActive(true);
            RoomStateHolder.instance.ChangeObjectState(5);
            RoomStateHolder.instance.ChangeObjectState(6);
            RoomStateHolder.instance.ChangeObjectState(7);
            return;
        }
        m_Currentstage = (stage)((int)m_Currentstage + 1);
        m_Grid.DeleteGrid();
        CreateLabyrinth();
        ResetBird();
    }
}
