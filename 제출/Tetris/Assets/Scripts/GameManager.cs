using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; set; }
    public static int Width = 10;
    public static int Height = 15;
    private int score = 0;
    public Transform[,] grid = new Transform[Width, Height];

    public Text gameName;
    public GameObject startButton;
    public Text GameOverText;
    public Text ScoreText;

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
        GameManagerAudioPlayer.PlayOneShot(LineClear);

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
        int NumOfDelete = 0;

        for (int y = 0; y < Height; y++)
        {
            if (IsRowFull(y))
            {
                NumOfDelete++;
                DeleteRow(y);
                DecreaseRowAbove(y + 1);
                --y;
            }
        }

        switch(NumOfDelete)
        {
            case 1:
                score += 100;
                break;
            case 2:
                score += 300;
                break;
            case 3:
                score += 500;
                break;
            case 4:
                score += 1000;
                break;
            default:
                break;
        }
    }

    public void GameStart()
    {
        gameName.gameObject.SetActive(false);
        startButton.SetActive(false);
        ScoreText.gameObject.SetActive(true);

        IsGameOver = false;
    }

    public void GameOver()
    {
        IsGameOver = true;
        GameOverText.gameObject.SetActive(true);
        GameManagerAudioPlayer.PlayOneShot(die);
    }

    private void Awake()
    {
        if (m_instance != null)
            Destroy(gameObject);

        IsGameOver = true;
        GameOverText.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);

        GameManagerAudioPlayer = GetComponent<AudioSource>();
        ScoreText.GetComponent<TextAsset>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && IsGameOver)
            SceneManager.LoadScene("MainScene");

        ScoreText.text = "Score : " + score;
    }
}
