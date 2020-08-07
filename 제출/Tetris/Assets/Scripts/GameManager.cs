using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    private int score = 0;
    public bool isGameover { get; private set; }

    public static GameManager instance
    {
        get
        {
            if(m_instance == null)
                m_instance = FindObjectOfType<GameManager>();

            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
    }
    
    void Start()
    {
        // 블록이 끝까지 쌓였을 때 게임오버 이벤트 추가
    }

    public void AddScore(int _score)
    {
        if (!isGameover)
        {
            score += _score;

            //UI 텍스트 업데이트
        }
    }
}
