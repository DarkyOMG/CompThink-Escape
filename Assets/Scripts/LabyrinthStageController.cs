using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum stage { One, Two, Three }
public class LabyrinthStageController : MonoBehaviour
{
    [SerializeField] private ActionListSO m_Dialogue;
    private Grid m_Grid;
    private Grid m_ProgrammingGrid;
    [SerializeField] private Transform m_GridParent;
    [SerializeField] private Transform m_ProgrammingGridParent;
    [SerializeField] private List<GameObject> m_Parts;
    [SerializeField] private HamsterController m_Hamster;
    private List<Vector2Int> m_Startingpoints = new List<Vector2Int> { new Vector2Int(9, 0), new Vector2Int(0, 0), new Vector2Int(3, 0) };
    private List<Vector2Int> m_Endpoints = new List<Vector2Int> { new Vector2Int(0, 6), new Vector2Int(9, 6), new Vector2Int(6, 6) };
    private stage m_Currentstage = stage.One;
    [SerializeField] GameObject m_WinModal;
    private List<int[]> m_CurrentLabyrinth;
    [SerializeField] private GameObject m_GoButton;

    private List<int[]> m_labyrinthOne = new List<int[]>
    {
        new int[10] {31,14,22,13,13,12,12,12,14,23},
        new int[10] {23,3,6,22,14,7,5,5,5,5},
        new int[10] {5,31,13,6,1,13,4,7,5,5},
        new int[10] {3,13,13,13,6,22,6,22,6,5},
        new int[10] {22,13,13,13,13,4,31,2,12,4},
        new int[10] {1,13,13,13,15,7,31,13,6,7}
    };
    private List<int[]> m_labyrinthTwo = new List<int[]>
    {
        new int[10] {22,13,12,14,23,31,12,12,13,14},
        new int[10] {7,22,6,1,0,15,5,7,23,5},
        new int[10] {22,6,22,6,5,23,1,14,1,6},
        new int[10] {5,23,5,31,2,6,7,5,7,23},
        new int[10] {3,6,3,13,13,13,12,2,13,4},
        new int[10] {31,13,13,13,13,13,6,31,13,4}
    };
    private List<int[]> m_labyrinthThree = new List<int[]>
    {
        new int[10] {22,14,23,31,12,15,22,13,12,14},
        new int[10] {5,7,5,31,4,23,3,14,7,7},
        new int[10] {1,13,2,14,5,1,14,5,22,14},
        new int[10] {1,13,15,5,3,4,5,5,5,7},
        new int[10] {3,15,22,6,31,6,5,5,5,23},
        new int[10] {31,13,2,13,13,13,0,2,2,6}
    };

    public Grid Grid => m_Grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateLabyrinth();
        Grid tempGrid = new Grid(2, 4, 75, m_ProgrammingGridParent,5);
        m_ProgrammingGrid = new Grid(2, 4, 75, m_ProgrammingGridParent,5);
        m_CurrentLabyrinth = m_labyrinthOne;

        for(int i = 0;i <2;i++){
            for (int j = 0; j < 4; j++)
            {
                tempGrid.InsertElement(i,j,m_Parts[j+(i*4)]);
                GameObject temp = m_ProgrammingGrid.InsertElement(i, j, m_Parts[8]);
                AlgorithmButton btnScript = temp.GetComponent<AlgorithmButton>();
                btnScript.m_Index = j+(i*4);
                btnScript.Hamster = m_Hamster;
            }
        }
        
        PlaceHamsterOnStart();
        ActionListEnumerator.instance.SetActionList(m_Dialogue);
        ActionListEnumerator.instance.StartActionList();
    }

    public LabElement GetElementAtCoordinate(Vector2Int coordinate)
    {
        LabElement temp = new LabElement();
        temp.type = m_CurrentLabyrinth[coordinate.y][coordinate.x] % 8;
        temp.orientation = (Orientation) (m_CurrentLabyrinth[coordinate.y][coordinate.x] / 8);
        return temp;
    }
    public Vector3 GetPositionAtCoordinate(Vector2Int coordinate)
    {

        return m_Grid.GetPositionOfElement(coordinate.x, coordinate.y);
    }

    public struct LabElement
    {
        public int type;
        public Orientation orientation;
    }

    public void PlaceHamsterOnStart()
    {
        m_Hamster.m_Position = m_Startingpoints[(int)m_Currentstage];
        m_Hamster.m_Orientation = Orientation.up;
        m_Hamster.gameObject.transform.eulerAngles = new Vector3(0, 0, (int)m_Hamster.m_Orientation * -90);
        m_Hamster.gameObject.transform.SetParent(m_GridParent);
        m_Hamster.gameObject.transform.localPosition = m_Grid.GetPositionOfElement(m_Startingpoints[(int)m_Currentstage].x, m_Startingpoints[(int)m_Currentstage].y);
    }
    public void StartHamster()
    {
        m_GoButton.SetActive(false);
        m_Hamster.StartHamster(m_Endpoints[(int)m_Currentstage]);
    }
    public void SendStopSignal()
    {
        m_Hamster.Running = false;
    }

    public void StopAndReset()
    {
        PlaceHamsterOnStart();
        m_GoButton.SetActive(true);
        
    }
    public void LabyrinthDone()
    {

        if ((int)m_Currentstage < 2)
        {
            m_Grid.DeleteGrid();
            m_Currentstage = (stage)((int)m_Currentstage + 1);
            CreateLabyrinth();
            m_CurrentLabyrinth = m_Currentstage == stage.One ? m_labyrinthOne : m_Currentstage == stage.Two ? m_labyrinthTwo : m_labyrinthThree;
            m_Hamster.Speed = (int)m_Currentstage+2;
            m_Hamster.transform.SetAsLastSibling();
            m_Hamster.EndPoint = m_Endpoints[(int)m_Currentstage];
            PlaceHamsterOnStart();
            return;
        }
        m_Hamster.Running = false;
        m_WinModal.SetActive(true);
        RoomStateHolder.instance.ChangeObjectState(3);
        RoomStateHolder.instance.ChangeObjectState(11);
    }
    private void CreateLabyrinth()
    {
        List<int[]> labyrinth = m_Currentstage == stage.One ? m_labyrinthOne : m_Currentstage == stage.Two ? m_labyrinthTwo : m_labyrinthThree;
        m_Grid = new Grid(10, 6, 100, m_GridParent);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject go = m_Grid.InsertElement(i, j, m_Parts[(labyrinth[j][i]) % 8]);
                go.transform.Rotate(new Vector3(0, 0, 1), -90.0f * ((int)labyrinth[j][i] / 8));
            }
        }
    }
}
