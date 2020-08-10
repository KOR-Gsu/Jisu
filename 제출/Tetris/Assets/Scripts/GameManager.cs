using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    
    public bool isGameover { get; private set; }
    public static int Width = 10;
    public static int Height = 15;
    public static Transform[,] grid = new Transform[Width, Height];

    public static GameManager instance
    {
        get
        {
            if(m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }

    public bool IsInside(Vector2 vec)
    {
        return (vec.x >= 0 && vec.x <= Width && vec.y >= 0);
    }

    public Vector2 roundVec2(Vector2 vec)
    {
        return new Vector2(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }

    private bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    private void DeleteRow(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    private void DecreaseRow(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void DecreaseRowAbove(int y)
    {
        for (int i = y; i < Height; i++)
            DecreaseRow(i);
    }

    public void DeleteFullRow()
    {
        for(int y = 0; y< Height;y++)
        {
            if(IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowAbove(y + 1);
                --y;
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
    }
}
