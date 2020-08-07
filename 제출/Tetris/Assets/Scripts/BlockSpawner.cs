using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BLOCK_TYPE
{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

public class blockSpawner : MonoBehaviour
{
    private float SpawnBetTime = 0.5f;
    private float SpawnLastTime = 0;

    private int NumofBlocks = 7;

    public List<Block> BlockList = new List<Block>();
    private List<Block> Blocks = new List<Block>();

    private void Awake()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
            return;
        
        for(int i = 0; i < 3; i++)
        {
            int type = Random.Range(0, 7);

            Block block = Instantiate(BlockList[type], transform.position, transform.rotation);
            block.gameObject.SetActive(false);

            Blocks.Add(block);
        }

    }

    private void Start()
    {
        Blocks[0].gameObject.SetActive(true);
    }
    
    void Update()
    {
        
    }

    
}
