using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileType { Ground, Obstacle}
[RequireComponent(typeof(Image))]
public class Tile : MonoBehaviour
{
    public Image m_image;

    [SerializeField] private AlgorithmStageController m_AlgorithmStageController;
    public AlgorithmStageController AlgorithmStageController { get => m_AlgorithmStageController; set { m_AlgorithmStageController = value; } }

    public TileType type;



    public Vector3 GetWorldPosition()
    {
        return m_AlgorithmStageController.GetPositionOfElement(gameObject);

    }
    public void Highlight(bool highlighted)
    {
        if (highlighted) 
        {
            m_image.color = Color.red;
        }
        else
        {
            m_image.color = Color.white;
        }
    }

}
