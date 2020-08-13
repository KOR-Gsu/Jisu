using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bStart : MonoBehaviour
{
    public void OnClickStart()
    {
        GameManager.instance.IsGameOver = false;
        FindObjectOfType<BlockSpawner>().SpawnNext();
    }
}
