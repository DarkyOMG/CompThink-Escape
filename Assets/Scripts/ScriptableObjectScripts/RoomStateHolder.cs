using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/RoomStateHolder")]
public class RoomStateHolder : SingletonScriptableObject<RoomStateHolder>
{
    [SerializeField] private int m_RoomState = 0;
    public int RoomState => m_RoomState;

    /*
    * Alter the current Game state by adding (bitwise-or) a reward int to the curren gamestate int.
    */
    public void ChangeObjectState(int objectIndex)
    {
        m_RoomState = m_RoomState | (1 << objectIndex);
        EventCollector.instance.OnRoomStateChanged?.Invoke();

    }
    public bool GetIndexState(int index)
    {
        return ((m_RoomState & (1 << index)) == 1);
    }
    public void Reset()
    {
        m_RoomState= 0;
    }

}
