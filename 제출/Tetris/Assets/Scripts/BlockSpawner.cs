using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Block[] Blocks;

    private void Awake()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
            return;
    }

    private void Start()
    {
        SpawnNext();
    }

    public void SpawnNext()
    {
        int type = Random.Range(0, Blocks.Length);

        Instantiate(Blocks[type], new Vector3(GameManager.Width / 2, GameManager.Height, 0), Quaternion.identity);
    }
}
