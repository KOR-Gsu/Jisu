using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;

    public float damageMax = 7;
    public float damageMin = 2;

    public float hpMax = 150;
    public float hpMin = 80;

    public float speedMax = 5;
    public float speedMin = 3;

    public Color strongEnemyColor = Color.green;

    private List<Enemy> enemies = new List<Enemy>();
    private int wave = 0;

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        if(enemies.Count <= 0)
        {
            
        }
    }

    private void SpawnEnemy()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for(int i =0; i< spawnCount;i++)
        {
            float intensity = Random.Range(0, 1.0f);
            CreateEnemy(intensity);
        }
    }

    private void CreateEnemy(float intensity)
    {
        float hp = Mathf.Lerp(hpMin, hpMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);


    }
}
