using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class UndergroundGrid : MonoBehaviour
{
    private int width = 32;
    private int depth = 128;

    public GameObject dirt;
    public GameObject marble;
    public GameObject gold;

    public float threshold = 0.3f;
    public float noiseScale = 0.01f;

    public float marbleThreshold = 0.1f;
    public float marbleNoiseScale = 0.001f;

    public float goldThreshold = 0.9f;
    public float goldNoiseScale = 0.3f;
    public float goldPropIncrease = 0.01f;

    private GameObject blocksGO;

    void Start()
    {
        blocksGO = Instantiate(new GameObject("Blocks"));
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                if (y < 2 || Mathf.PerlinNoise(x * noiseScale, y * noiseScale) > threshold)
                {
                    GameObject block;
                    if (Mathf.PerlinNoise(x * marbleNoiseScale, y * marbleNoiseScale) > marbleThreshold)
                        block = marble;
                    else if (Mathf.PerlinNoise(x * goldNoiseScale, y * goldNoiseScale) > goldThreshold - goldPropIncrease * y)
                        block = gold;
                    else
                        block = dirt;

                    var newBlock = Instantiate(block, new Vector3(x, -y, 0), Quaternion.identity, blocksGO.transform);
                    newBlock.gameObject.AddComponent<BoxCollider2D>();
                    newBlock.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    newBlock.GetComponent<SpriteRenderer>().flipY = Random.value > 0.5;
                    newBlock.transform.rotation = Quaternion.Euler(0,0, Mathf.FloorToInt(Random.Range(0, 3)) * 90);
                }
                
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - Mathf.Round(Time.time) < 0.01f)
        {
            Destroy(blocksGO);
            Start();
        }
    }
}