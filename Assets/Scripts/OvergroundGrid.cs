using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergroundGrid : MonoBehaviour
{
    public int width = 32;
    public int height = 128;
    public int startHeight = 20;
    public float cloudChance = 2f/32f;
    public float cloud1Chance = 0.33f;
    public float cloud2Chance = 0.66f;
    public float cloud3Chance = 1.0f;
    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int y = startHeight; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.value < cloudChance)
                {
                    float cloudSpecifier = Random.value;
                    if (cloudSpecifier < cloud1Chance)
                    {
                        Instantiate(cloud1, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (cloudSpecifier < cloud2Chance)
                    {
                        Instantiate(cloud2, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (cloudSpecifier < cloud3Chance)
                    {
                        Instantiate(cloud3, new Vector3(x, y, 0), Quaternion.identity);
                    }

                    x += 5;
                    y += 2;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
