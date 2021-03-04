using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Define.EnemyType enemyType;
    [SerializeField] private int maxCount = 4;
    [SerializeField] private int keepSpawnCount = 2;
    [SerializeField] private int curSpawnCount = 0;
    [SerializeField] private float spawnRadius = 7.0f;
    [SerializeField] private float spawnTime = 4.0f;
    private int _keepSpawnCount = 0;

    private List<GameObject> enemyPrefabList = new List<GameObject>();
    private List<Enemy> enemyList = new List<Enemy>();

    private int spawnIndex = 0;
    private string type;

    public void UpdateCurSpawnCount(int count) { curSpawnCount += count; }

    private void Start()
    {
        SetEnemies();

        for(int i = 0; i < keepSpawnCount; i++)
        {
            Vector3 randSpawnPoint;
            NavMeshAgent pathFinder = enemyList[i].GetComponent<NavMeshAgent>();
            while (true)
            {
                Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
                randDir.y = enemyList[i].transform.position.y;
                randSpawnPoint = transform.position + randDir;

                NavMeshPath path = new NavMeshPath();
                if (pathFinder.CalculatePath(randSpawnPoint, path))
                    break;
            }

            enemyPrefabList[i].transform.position = randSpawnPoint;
            enemyPrefabList[i].SetActive(true);
        }
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        int index = 0, check = 0;
        bool exist = false;
        while (check < maxCount)
        {
            if (index >= maxCount)
                index = 0;

            if(enemyList[index].dead)
            {
                curSpawnCount -= 1;
                exist = true;
            }
            else
            {
                if (exist && !enemyPrefabList[index].activeSelf)
                {
                    spawnIndex = index;
                    break;
                }
            }
            index++;
            check++;
        }

        while (_keepSpawnCount + curSpawnCount < keepSpawnCount)
        {
            StartCoroutine(Spawn());
        }
    }

    private void SetEnemies()
    {
        MonsterDataJson monsterDataJson = DataManager.instance.JsonToData<MonsterDataJson>(DataManager.instance.monsterDataFileName);
        string path = "Prefab/";

        switch (enemyType)
        {
            case Define.EnemyType.warrior:
                type = "warrior";
                break;
            case Define.EnemyType.archer:
                type = "archer";
                break;
        }

        monsterDataJson.monsterDataDictionary.TryGetValue(type, out MonsterData data);

        for (int i = 0; i < maxCount; i++)
        {
            GameObject newEnemyPrefab = ResourceManager.instance.Instantiate(path + type, transform);
            Enemy newEnemy = newEnemyPrefab.GetComponent<Enemy>();

            newEnemyPrefab.SetActive(false);
            newEnemy.Initializing(data);
            enemyPrefabList.Add(newEnemyPrefab);
            enemyList.Add(newEnemy);
        }
    }

    private IEnumerator Spawn()
    {
        _keepSpawnCount++;
        yield return new WaitForSeconds(Random.Range(0, spawnTime));

        Vector3 randSpawnPoint;
        NavMeshAgent pathFinder = enemyList[spawnIndex].GetComponent<NavMeshAgent>();
        while(true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
            randDir.y = enemyList[curSpawnCount].transform.position.y;
            randSpawnPoint = transform.position + randDir;

            NavMeshPath path = new NavMeshPath();
            if (pathFinder.CalculatePath(randSpawnPoint, path))
                break;
        }

        enemyPrefabList[spawnIndex].transform.position = randSpawnPoint;
        enemyPrefabList[spawnIndex].SetActive(true);
        curSpawnCount++;
        _keepSpawnCount--;
    }
}
