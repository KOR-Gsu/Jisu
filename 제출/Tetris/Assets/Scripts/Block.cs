using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float FallBetTime = 0.5f;

    private bool isStopped = false;

    private AudioSource BlockAudioPlayer;
    public AudioClip DropClip;

    private BoxCollider2D BlockCollider;

    // Start is called before the first frame update
    void Start()
    {
        BlockAudioPlayer = GetComponent<AudioSource>();
        BlockCollider = GetComponent<BoxCollider2D>();

        StartCoroutine(Falling());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Falling()
    {
        if(GameManager.instance != null && GameManager.instance.isGameover)
        {

            yield return new WaitForSeconds(FallBetTime);
        }
    }
}
