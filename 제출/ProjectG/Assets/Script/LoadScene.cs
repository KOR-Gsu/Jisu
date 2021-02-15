using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "VillageScene")
            {
                SceneManager.UnloadSceneAsync("VillageScene");
                SceneManager.LoadSceneAsync("DungeonScene");
            }

            if (SceneManager.GetActiveScene().name == "DungeonScene")
            {
                SceneManager.UnloadSceneAsync("DungeonScene");
                SceneManager.LoadSceneAsync("VillageScene");
            }
        }
    }
}
