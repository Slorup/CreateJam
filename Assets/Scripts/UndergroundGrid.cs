using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class UndergroundGrid : MonoBehaviour
{
    private int width = 32;
    private int depth = 128;

    public GameObject block;

    public float threshold = 0.3f;
    public float noiseScale = 0.01f;

    private Block[,] blocks;
    
    void Start()
    {
        blocks = new Block[width, depth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                if (Mathf.PerlinNoise(x * noiseScale, y * noiseScale) > threshold)
                {
                    blocks[x, y] = Instantiate(block, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Block>();
                    blocks[x, y].tag = "Block";
                    blocks[x, y].gameObject.AddComponent<BoxCollider2D>();

                }
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}