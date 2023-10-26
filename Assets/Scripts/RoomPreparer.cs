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
        Debug.Log("Preparing Room");
        // Unlock all buttons and prefabs that are allready unlocked according to the game state.
       for (int i = 0; i < m_ChangeableObjects.Count; i++)
        {
            if ((m_RoomStateHolder.RoomState & (1 << i)) != 0)
            {
                m_ChangeableObjects[i].SetActive(true);
            }
            else
            {
                m_ChangeableObjects[i].SetActive(false);
            }
        }
    }

}
