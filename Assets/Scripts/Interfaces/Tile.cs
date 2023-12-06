using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileType { Ground, Obstacle}
[RequireComponent(typeof(Image))]
public class Tile : MonoBehaviour
{
    public Image m_image;
    public Sprite m_StandardImage;
    public bool m_IsBlue;
    public Sprite m_AlternateImage;
    [SerializeField] private AlgorithmStageController m_AlgorithmStageController;
    public AlgorithmStageController AlgorithmStageController { get => m_AlgorithmStageController; set { m_AlgorithmStageController = value; } }

    public TileType type;



    public void Start()
    {
        if(type == TileType.Obstacle){
            GetComponent<Image>().sprite = Random.Range(0, 100)%2 == 1? m_StandardImage:m_AlternateImage;
        }
    }
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
            m_image.color = m_IsBlue? Color.blue: Color.white;
        }
    }

}
