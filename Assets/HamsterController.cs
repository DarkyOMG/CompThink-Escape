using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Orientation { up,right,down,left }
enum HamsterAction { forward, goLeft, turnAround, goRight}
public class HamsterController : MonoBehaviour
{
    public LabyrinthStageController m_LabyrinthStageController;
    public Vector2Int m_Position;
    public Orientation m_Orientation = Orientation.up;
    private AnimalMover m_Mover;
    [SerializeField] private ColorFlash m_flash;
    private int[] m_TurnaroundLookuptable = { 0, 4, 0, 4, 1, 5, 1, 5, };
    private int m_MoveCounter = 0;
    private List<List<HamsterAction>> m_WrongMoveTable = new List<List<HamsterAction>>()
    {
        new List<HamsterAction>{ },
        new List<HamsterAction>{HamsterAction.goLeft },
        new List<HamsterAction>{HamsterAction.forward },
        new List<HamsterAction>{HamsterAction.goLeft,HamsterAction.forward },
        new List<HamsterAction>{HamsterAction.goRight },
        new List<HamsterAction>{HamsterAction.goLeft,HamsterAction.goRight },
        new List<HamsterAction>{HamsterAction.goRight,HamsterAction.forward },
        new List<HamsterAction>{HamsterAction.goLeft,HamsterAction.forward,HamsterAction.goRight }
    };

    [SerializeField] private HamsterAction[] m_hamsterActions = new HamsterAction[8];
    private bool m_Running = false;
    public bool Running { get => m_Running; set { m_Running = value; }  }
    private Vector2Int m_EndPoint;
    public Vector2Int EndPoint { get => m_EndPoint; set { m_EndPoint = value; } }
    public float Speed { get => m_Mover.Speed; set { m_Mover.Speed = value; } }

    private void Start()
    {
        m_Mover = GetComponent<AnimalMover>();
    }
    public void SetAction(int actionIndex)
    {
        m_hamsterActions[actionIndex] = (HamsterAction) (((int)m_hamsterActions[actionIndex] + 1) % 4);
    }
    public int GetAction(int actionIndex)
    {
        
        return (int)m_hamsterActions[actionIndex];
    }
    public void StartHamster(Vector2Int endpoint)
    {
        EventCollector.instance.OnAnimalReachedEnd += MakeNextMove;
        m_Running = true;
        m_EndPoint = endpoint;
        m_MoveCounter = 0;
        MakeNextMove();
    }
    public void MakeNextMove()
    {
        m_MoveCounter++;
        if (!m_Running || m_MoveCounter > 100)
        {
            EventCollector.instance.OnAnimalReachedEnd -= MakeNextMove;
            m_LabyrinthStageController.StopAndReset();
            return;
        }
        LabyrinthStageController.LabElement field = m_LabyrinthStageController.GetElementAtCoordinate(m_Position);

        int realOrientation = m_Orientation - field.orientation;
        int realtype;
        switch (realOrientation)
        {
            case 1 or -3:
                realtype = (field.type / 2) % 8;
                break;
            case 2 or -2:
                realtype = m_TurnaroundLookuptable[field.type];
                break;
            case 3 or -1:
                realtype = (field.type * 2) % 8;
                break;
            default:
                realtype = field.type;
                break;
        }
        if ((int)m_hamsterActions[realtype] > 0)
        {
            m_Orientation = (int)m_hamsterActions[realtype] == 2 ? (Orientation)(((int)m_Orientation + 2) % 4) : (Orientation)((((int)m_Orientation + ((int)m_hamsterActions[realtype] - 2) % 4) +4)%4);
            gameObject.transform.eulerAngles = new Vector3(0, 0, (int)m_Orientation * -90);
            

        }
        if((int)m_hamsterActions[realtype] != 2)
        {

            int nextX = (((int)m_Orientation - 2) % 2) * (-1) + m_Position.x;
            int nextY = (((int)m_Orientation - 1) % 2) * (-1) + m_Position.y;
            m_Position.x = nextX;
            m_Position.y = nextY;

            if (m_Position == m_EndPoint)
            {
                m_flash.FlashColor(Color.green);
                m_LabyrinthStageController.LabyrinthDone();
                EventCollector.instance.OnAnimalReachedEnd.Invoke();
                return;
            }
            Vector3 target;
            if (m_WrongMoveTable[realtype].Contains(m_hamsterActions[realtype]))
            {
                target = m_Position.x > 9 || m_Position.y > 5 ||  m_Position.y < 0 || m_Position.x < 0 ? transform.localPosition + new Vector3(1, 1, 1) : transform.localPosition + (m_LabyrinthStageController.GetPositionAtCoordinate(m_Position) - transform.localPosition) / 2.0f;
                m_Running = false;
                m_flash.FlashColor(Color.red);
            } 
            else
            {
                target = m_LabyrinthStageController.GetPositionAtCoordinate(m_Position);
            }
            m_MoveCounter =0;
            m_Mover.SetTarget(target);
        } else
        {
            EventCollector.instance.OnAnimalReachedEnd.Invoke();
        }
    }
}
