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
            DontDestroyOnLoad(FindObjectOfType<DataManager>());

            if (SceneManager.GetActiveScene().name == "TownScene")
                SceneManager.LoadScene("DungeonScene");

            if (SceneManager.GetActiveScene().name == "DungeonScene")
                SceneManager.LoadScene("TownScene");
        }
    }
}
