using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsGameover { get; private set; }
    public static int Width = 10;
    public static int Height = 15;
    public static Transform[,] grid = new Transform[Width, Height];

    private static AudioSource GameManagerAudioPlayer;
    public static AudioClip LineClear;

    public static bool IsInside(Vector2 vec)
    {
        return (vec.x >= 0 && vec.x < Width && vec.y >= 0);
    }

    public static Vector2 roundVec2(Vector2 vec)
    {
        return new Vector2(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }

    private static bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    private static void DeleteRow(int y)
    {
        GameManagerAudioPlayer.PlayOneShot(LineClear);

        for (int x = 0; x < Width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    private static void DecreaseRow(int y)
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

    private static void DecreaseRowAbove(int y)
    {
        for (int i = y; i < Height; i++)
            DecreaseRow(i);
    }

    public static void DeleteFullRow()
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
        //if (GetInstance != null)
        //    Destroy(gameObject);

        IsGameover = false;

        GameManagerAudioPlayer = GetComponent<AudioSource>();
    }
}
