using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; set; }
    public static int Width = 10;
    public static int Height = 15;
    public Transform[,] grid = new Transform[Width, Height];

    public GameObject gameName;
    public GameObject startButton;

    private AudioSource GameManagerAudioPlayer;
    public AudioClip LineClear;
    public AudioClip die;

    private static GameManager m_instance = null;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }


    public bool IsInside(Vector2 vec)
    {
        return (vec.x >= 0 && vec.x < Width && vec.y >= 0);
    }

    public  Vector2 roundVec2(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }

    private  bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    private  void DeleteRow(int y)
    {
        //GameManagerAudioPlayer.PlayOneShot(LineClear);

        for (int x = 0; x < Width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    private  void DecreaseRow(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private  void DecreaseRowAbove(int row)
    {
        for (int y = row; y < Height; y++)
            DecreaseRow(y);
    }

    public  void DeleteFullRow()
    {
        for (int y = 0; y < Height; y++)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowAbove(y + 1);
                --y;
            }
        }
    }

    public void GameOver()
    {
        GameManagerAudioPlayer.PlayOneShot(die);

        if(Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("GameScene");
    }

    private void Awake()
    {
        if (m_instance != null)
            Destroy(gameObject);

        IsGameOver = true;

        GameManagerAudioPlayer = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(!IsGameOver)
        {
            gameName.SetActive(false);
            startButton.SetActive(false);
        }
        else
        {
            gameName.SetActive(true);
            startButton.SetActive(true);
        }

    }
}
