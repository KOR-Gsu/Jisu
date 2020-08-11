﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Block[] Blocks;

    private void Start()
    {
        SpawnNext();
    }

    public void SpawnNext()
    {
        int type = Random.Range(0, Blocks.Length);

        Instantiate(Blocks[type], new Vector3(GameManager.Width / 2 - 1, GameManager.Height - 1, 0), Quaternion.identity);
    }
}
