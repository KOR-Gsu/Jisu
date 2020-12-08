using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public CanvasGroup fadeCg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.UnloadSceneAsync("VillageScene");
            SceneManager.LoadSceneAsync("DungeonScene");
        }
    }
}
