using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomPreparer : MonoBehaviour
{

    [SerializeField] private RoomStateHolder m_RoomStateHolder;
    [SerializeField] private List<GameObject> m_ChangeableObjects;

    private void OnEnable()
    {
        EventCollector.instance.OnRoomStateChanged += PrepareRoom;
    }
    private void OnDisable()
    {
        EventCollector.instance.OnRoomStateChanged -= PrepareRoom;
    }
    // Start is called before the first frame update
    void Start()
    {
        PrepareRoom();
    }
    private void OnApplicationQuit()
    {
        m_RoomStateHolder.Reset();
    }

    public void PrepareRoom()
    {
        Debug.Log("Testing hiere");
        // Unlock all buttons and prefabs that are allready unlocked according to the game state.
       for (int i = 0; i < m_ChangeableObjects.Count; i++)
        {
            Debug.Log(i + " "+RoomStateHolder.instance.GetIndexState(i));
            m_ChangeableObjects[i].SetActive(RoomStateHolder.instance.GetIndexState(i));
        }
    }

}
