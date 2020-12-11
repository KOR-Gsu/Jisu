using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }
    public bool isGameOver { get; private set; }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        FindObjectOfType<PlayerHP>().onDeath += EndGame;
    }
    
    void EndGame()
    {
        isGameOver = true;
    }
}
