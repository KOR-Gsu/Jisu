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
            DontDestroyOnLoad(DataManager.instance);

            if (SceneManager.GetActiveScene().name == "VillageScene")
                SceneManager.LoadScene("DungeonScene");

            if (SceneManager.GetActiveScene().name == "DungeonScene")
                SceneManager.LoadScene("VillageScene");
        }
    }
}
