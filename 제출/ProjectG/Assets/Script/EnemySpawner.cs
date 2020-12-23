using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;

    public float damageMax = 5;
    public float damageMin = 2;

    public float hpMax = 150;
    public float hpMin = 80;

    public float speedMax = 5;
    public float speedMin = 3;

    public Color strongEnemyColor = Color.green;

    private List<Enemy> enemies = new List<Enemy>();
    private int wave = 0;
    private int curSpawn = 3;

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        if(enemies.Count <= 0)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        wave++;

        //int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        //int spawnCount = 4;

        int spawnCount = 1;
        for (int i =0; i< spawnCount;i++)
        {
            float intensity = Random.Range(0, 1.0f);
            CreateEnemy(intensity);
        }
        //curSpawn = 0;
    }

    private void CreateEnemy(float intensity)
    {
        float startingHp = Mathf.Lerp(hpMin, hpMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);

        Transform spawnPoint = spawnPoints[curSpawn];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(startingHp, damage, speed);
        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 5f);

        enemies.Add(enemy);
        //curSpawn++;
    }
}
