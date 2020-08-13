using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float FallBetTime = 1f;
    private float FallLastTime = 0;
    
    private AudioSource BlockAudioPlayer;
    public AudioClip DropClip;
    
    private bool IsValidChildPos()
    {
        foreach(var child in transform)
        {
            Vector2 vec = GameManager.instance.roundVec2((child as Transform).position);

            if (!GameManager.instance.IsInside(vec))
                return false;

            if (GameManager.instance.grid[(int)vec.x, (int)vec.y] != null && GameManager.instance.grid[(int)vec.x, (int)vec.y].parent != transform)
                return false;
        }

        return true;
    }

    private void UpdateGrid()
    {
        for(int y = 0; y < GameManager.Height; y++)
        {
            for(int x = 0; x < GameManager.Width; x++)
            {
                if (GameManager.instance.grid[x, y] != null)
                {
                    if (GameManager.instance.grid[x, y] != null)
                    {
                        if (GameManager.instance.grid[x, y].parent == transform)
                            GameManager.instance.grid[x, y] = null;
                    }
                }
            }
        }

        foreach(Transform child in transform)
        {
            Vector2 vec = GameManager.instance.roundVec2(child.position);
            GameManager.instance.grid[(int)vec.x, (int)vec.y] = child;
        }
    }

    private void Start()
    {
        BlockAudioPlayer = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidChildPos())
                UpdateGrid();
            else
                transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidChildPos())
                UpdateGrid();
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);

            if (IsValidChildPos())
                UpdateGrid();
            else
                transform.Rotate(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - FallLastTime >= FallBetTime))
        {
            transform.position += new Vector3(0, -1, 0);

            if (IsValidChildPos())
                UpdateGrid();
            else
            {
                transform.position += new Vector3(0, 1, 0);

                GameManager.instance.DeleteFullRow();

                FindObjectOfType<BlockSpawner>().SpawnNext();

                enabled = false;

                BlockAudioPlayer.PlayOneShot(DropClip);
            }

            FallLastTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (true)
            {
                transform.position += new Vector3(0, -1, 0);

                if (IsValidChildPos())
                    UpdateGrid();
                else
                {
                    transform.position += new Vector3(0, 1, 0);

                    GameManager.instance.DeleteFullRow();

                    FindObjectOfType<BlockSpawner>().SpawnNext();

                    enabled = false;

                    BlockAudioPlayer.PlayOneShot(DropClip);

                    break;
                }
            }
            FallLastTime = Time.time;
        }
    }
}
