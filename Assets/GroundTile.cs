using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroundTile : Tile
{


    private Button m_Button;

    public Vector2 GetPosition()
    {
        throw new System.NotImplementedException();
    }

    TileType GetTileType()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.type = TileType.Ground;
        m_Button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
