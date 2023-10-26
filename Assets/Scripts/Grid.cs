using UnityEngine;

public class Grid
{
    private int m_Width;
    private int m_Height;
    private float m_FieldSize;
    private int m_GapSize;
    private Transform m_Parent;
    public float FieldSize { get => m_FieldSize; }
    public int Width { get => m_Width; }
    public int Height { get => m_Height; }
    public int GapSize { get => m_GapSize; }

    private GameObject[,] gridArray;
    
    public Grid(int width, int height, float fieldSize,Transform parent, int gapSize = 0)
    {
        this.m_Width = width;
        this.m_Height = height;
        this.m_FieldSize = fieldSize;
        this.m_Parent = parent;
        this.m_GapSize = gapSize;

        gridArray = new GameObject[width, height];
    }

    public GameObject InsertElement(int x, int y, GameObject prefab)     
    {
        GameObject element = MonoBehaviour.Instantiate(prefab,m_Parent);
        element.transform.localPosition = new Vector3((x * m_FieldSize) + x*m_GapSize , (y * m_FieldSize) + y* m_GapSize, 0f);
        element.GetComponent<RectTransform>().sizeDelta = new Vector2(m_FieldSize, m_FieldSize);
        element.transform.SetParent(m_Parent,false);
        gridArray[x,y] = element;
        return element;
    }

    public GameObject GetElementAtIndex(int x, int y)
    {
        return gridArray[x, y];
    }
    public Vector3 GetPositionOfElement(int x, int y)
    {
        return GetElementAtIndex(x, y).gameObject.transform.localPosition;
    }
    public Vector2Int GetIndexByElement(GameObject element)
    {
        Vector2Int temp = new Vector2Int();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (gridArray[i, j] == element)
                {
                    temp = new Vector2Int(i, j);
                }
            }
        }
        Debug.Log(temp);
        return temp;
    }

    public void DeleteGrid()
    {
        foreach(GameObject go in gridArray)
        {
            MonoBehaviour.Destroy(go);
        }
    }

    public void PlaceTile(int x, int y, GameObject element)
    {
        gridArray[x, y] = element;
    }

}
